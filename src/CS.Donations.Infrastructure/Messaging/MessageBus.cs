using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using CS.Donations.Application.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace CS.Donations.Infrastructure.Messaging;

public class MessageBus : IMessageBus
{
    private readonly ILogger<MessageBus> _logger;
    private readonly ConnectionFactory _factory;
    private readonly int _retryCount;
    private IConnection? _connection;
    private bool _isConnected;
    private readonly List<IModel> _channels = new();

    public MessageBus(IConfiguration configuration, ILogger<MessageBus> logger)
    {
        var host = configuration["MessageBusConfigs:Host"]
            ?? throw new InvalidOperationException("MessageBusConfigs:Host não configurado.");
        _retryCount = int.Parse(configuration["MessageBusConfigs:RetryCount"] ?? "5");
        _logger = logger;
        _factory = new ConnectionFactory { Uri = new Uri(host) };
    }

    private IConnection Connection
    {
        get
        {
            if (!_isConnected || _connection is null || !_connection.IsOpen)
                Connect();
            return _connection!;
        }
    }

    public void Connect()
    {
        var policy = Policy.Handle<SocketException>().Or<BrokerUnreachableException>()
            .WaitAndRetry(_retryCount, op => TimeSpan.FromSeconds(Math.Pow(2, op)),
                (ex, _) => _logger.LogError(ex, "Couldn't connect to RabbitMQ"));
        policy.Execute(() =>
        {
            _connection = _factory.CreateConnection();
            _isConnected = true;
        });
    }

    public async Task Subscribe<TMessage>(string exchange, string routingKey, Func<TMessage, Task> handler, CancellationToken stoppingToken)
    {
        await Task.Yield();
        var channel = Connection.CreateModel();
        _channels.Add(channel);

        channel.ExchangeDeclare(exchange + "-exchange", ExchangeType.Direct, durable: true);
        channel.QueueDeclare(routingKey + "-queue", durable: true, exclusive: false, autoDelete: false);
        channel.QueueBind(routingKey + "-queue", exchange + "-exchange", routingKey);
        channel.BasicQos(0, 1, false);

        var consumer = new EventingBasicConsumer(channel);
        channel.BasicConsume(routingKey + "-queue", autoAck: false, consumer);

        consumer.Received += async (_, ea) =>
        {
            try
            {
                var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                var msg = JsonSerializer.Deserialize<TMessage>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                _logger.LogInformation("[SUBSCRIBE] {RoutingKey} {Message}", routingKey, json);
                await handler(msg!);
                channel.BasicAck(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[SUBSCRIBE] error {RoutingKey}", routingKey);
                channel.BasicNack(ea.DeliveryTag, false, true);
            }
        };
    }

    public void Dispose()
    {
        foreach (var ch in _channels)
        {
            try { ch.Dispose(); } catch { }
        }
        _connection?.Dispose();
    }
}
