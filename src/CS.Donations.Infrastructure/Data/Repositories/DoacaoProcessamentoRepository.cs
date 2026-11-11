using CS.Donations.Domain.Entities;
using CS.Donations.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CS.Donations.Infrastructure.Data.Repositories;

public class DoacaoProcessamentoRepository : IDoacaoProcessamentoRepository
{
    private readonly AppDbContext _db;
    public DoacaoProcessamentoRepository(AppDbContext db) => _db = db;

    public Task<DoacaoEntity?> GetDoacaoAsync(Guid doacaoId, CancellationToken ct = default)
        => _db.Doacoes.FirstOrDefaultAsync(d => d.Id == doacaoId, ct);

    public Task<CampanhaEntity?> GetCampanhaAsync(Guid campanhaId, CancellationToken ct = default)
        => _db.Campanhas.FirstOrDefaultAsync(c => c.Id == campanhaId, ct);

    public Task SaveChangesAsync(CancellationToken ct = default) => _db.SaveChangesAsync(ct);
}
