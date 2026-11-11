using CS.Donations.Domain.Exceptions;

namespace CS.Donations.Domain.Entities;

public class CampanhaEntity
{
    public Guid Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public decimal MetaFinanceira { get; set; }
    public string Status { get; set; } = "Ativa";
    public decimal ValorTotalArrecadado { get; set; }
    public DateTime CriadoEm { get; set; }
    public DateTime AtualizadoEm { get; set; }

    public void IncrementarArrecadacao(decimal valor)
    {
        if (valor <= 0)
            throw new DomainException("Valor inválido");
        ValorTotalArrecadado += valor;
        AtualizadoEm = DateTime.UtcNow;
    }
}
