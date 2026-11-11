using CS.Donations.Application.Messaging;
using CS.Donations.Application.Messaging.Consumers;
using CS.Donations.Application.Messaging.Events;
using Microsoft.Extensions.Options;

namespace CS.Donations.Worker;

public class DoacaoRecebidaWorker : BackgroundService
{
    private readonly IServiceProvider _sp;
    private readonly ILogger<DoacaoRecebidaWorker> _logger;
    private readonly DoacaoRecebidaWorkerConfig _config;

    public DoacaoRecebidaWorker(IServiceProvider sp, ILogger<DoacaoRecebidaWorker> logger, IOptions<DoacaoRecebidaWorkerConfig> config)
    {
        _sp = sp;
        _logger = logger;
        _config = config.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                if (!_config.Ativo)
                {
                    _logger.LogInformation("Worker desativado");
                    return;
                }

                _logger.LogInformation("Worker doações iniciado");
                using var scope = _sp.CreateScope();
                var bus = scope.ServiceProvider.GetRequiredService<IMessageBus>();
                await bus.Subscribe<DoacaoRecebidaEvent>(_config.Exchange, _config.RoutingKey, Processar, stoppingToken);
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Worker exception; retry em 5s");
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }

    private async Task Processar(DoacaoRecebidaEvent dados)
    {
        using var scope = _sp.CreateScope();
        var consumer = scope.ServiceProvider.GetRequiredService<IDoacaoRecebidaMessage>();
        await consumer.Consumir(dados);
    }
}
