namespace CS.Donations.Worker;

public class DoacaoRecebidaWorkerConfig
{
    public bool Ativo { get; set; } = true;
    public string Exchange { get; set; } = "cs.doacao.recebida";
    public string RoutingKey { get; set; } = "donations.doacao-recebida";
}
