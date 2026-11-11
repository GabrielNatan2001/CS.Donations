using CS.Donations.Application.Messaging.Events;
using CS.Donations.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace CS.Donations.Application.Messaging.Consumers;

public class DoacaoRecebidaConsumer : IDoacaoRecebidaMessage
{
    private readonly IDoacaoProcessamentoRepository _repo;
    private readonly ILogger<DoacaoRecebidaConsumer> _logger;

    public DoacaoRecebidaConsumer(IDoacaoProcessamentoRepository repo, ILogger<DoacaoRecebidaConsumer> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task Consumir(DoacaoRecebidaEvent dados)
    {
        var doacao = await _repo.GetDoacaoAsync(dados.DoacaoId);
        if (doacao is null)
        {
            _logger.LogWarning("Doação {DoacaoId} não encontrada", dados.DoacaoId);
            return;
        }

        if (doacao.JaProcessada)
        {
            _logger.LogInformation("Doação {DoacaoId} já processada (idempotência)", dados.DoacaoId);
            return;
        }

        var campanha = await _repo.GetCampanhaAsync(dados.CampanhaId);
        if (campanha is null)
        {
            _logger.LogWarning("Campanha {CampanhaId} não encontrada", dados.CampanhaId);
            return;
        }

        campanha.IncrementarArrecadacao(doacao.Valor);
        doacao.MarcarProcessada();
        await _repo.SaveChangesAsync();

        _logger.LogInformation(
            "Doação {DoacaoId} processada. Campanha {CampanhaId} total={Total}",
            doacao.Id, campanha.Id, campanha.ValorTotalArrecadado);
    }
}
