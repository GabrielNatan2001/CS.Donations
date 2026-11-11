using CS.Donations.Application.Messaging.Events;

namespace CS.Donations.Application.Messaging.Consumers;

public interface IDoacaoRecebidaMessage
{
    Task Consumir(DoacaoRecebidaEvent dados);
}
