namespace CS.Donations.Application.Messaging;

public interface IMessageBus : IDisposable
{
    Task Subscribe<TMessage>(string exchange, string routingKey, Func<TMessage, Task> handler, CancellationToken stoppingToken);
}
