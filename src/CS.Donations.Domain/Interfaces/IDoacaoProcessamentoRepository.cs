using CS.Donations.Domain.Entities;

namespace CS.Donations.Domain.Interfaces;

public interface IDoacaoProcessamentoRepository
{
    Task<DoacaoEntity?> GetDoacaoAsync(Guid doacaoId, CancellationToken ct = default);
    Task<CampanhaEntity?> GetCampanhaAsync(Guid campanhaId, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}
