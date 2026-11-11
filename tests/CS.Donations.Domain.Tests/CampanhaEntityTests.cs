using CS.Donations.Domain.Entities;
using CS.Donations.Domain.Exceptions;

namespace CS.Donations.Domain.Tests;

public class CampanhaEntityTests
{
    [Fact]
    public void Deve_incrementar_arrecadacao()
    {
        var c = new CampanhaEntity { ValorTotalArrecadado = 10 };
        c.IncrementarArrecadacao(25);
        Assert.Equal(35, c.ValorTotalArrecadado);
    }

    [Fact]
    public void Deve_rejeitar_valor_zero()
    {
        var c = new CampanhaEntity();
        Assert.Throws<DomainException>(() => c.IncrementarArrecadacao(0));
    }
}
