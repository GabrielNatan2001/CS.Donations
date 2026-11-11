using CS.Donations.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CS.Donations.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<CampanhaEntity> Campanhas => Set<CampanhaEntity>();
    public DbSet<DoacaoEntity> Doacoes => Set<DoacaoEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CampanhaEntity>(b =>
        {
            b.ToTable("campanhas");
            b.HasKey(x => x.Id);
            b.Property(x => x.Titulo).HasMaxLength(200);
            b.Property(x => x.Descricao).HasMaxLength(2000);
            b.Property(x => x.MetaFinanceira).HasColumnType("numeric(18,2)");
            b.Property(x => x.ValorTotalArrecadado).HasColumnType("numeric(18,2)");
            b.Property(x => x.Status).HasMaxLength(50);
        });

        modelBuilder.Entity<DoacaoEntity>(b =>
        {
            b.ToTable("doacoes");
            b.HasKey(x => x.Id);
            b.Property(x => x.Valor).HasColumnType("numeric(18,2)");
            b.Property(x => x.Status).HasConversion<string>().HasMaxLength(50);
        });
    }
}
