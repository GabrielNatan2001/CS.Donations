namespace CS.Donations.Application.Messaging.Events;

public record DoacaoRecebidaEvent(
    Guid DoacaoId,
    Guid CampanhaId,
    Guid DoadorId,
    decimal Valor,
    DateTime DataHoraUtc);
