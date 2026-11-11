using CS.Donations.Domain.Common;

namespace CS.Donations.Domain.Entities;

public class DoacaoEntity
{
    public Guid Id { get; set; }
    public Guid CampanhaId { get; set; }
    public Guid DoadorId { get; set; }
    public decimal Valor { get; set; }
    public StatusDoacao Status { get; set; }
    public DateTime CriadoEm { get; set; }

    public bool JaProcessada => Status == StatusDoacao.Processada;

    public void MarcarProcessada()
    {
        if (JaProcessada) return;
        Status = StatusDoacao.Processada;
    }
}
