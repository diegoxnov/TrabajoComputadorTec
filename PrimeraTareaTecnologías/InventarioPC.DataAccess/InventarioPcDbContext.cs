using InventarioPC.Domain;
using Microsoft.EntityFrameworkCore;

namespace InventarioPC.DataAccess;

public class InventarioPcDbContext : DbContext
{
    public InventarioPcDbContext(DbContextOptions<InventarioPcDbContext> options) : base(options)
    {
    }

    public DbSet<Producto> Productos => Set<Producto>();
    public DbSet<Armado> Armados => Set<Armado>();
    public DbSet<ArmadoDetalle> ArmadoDetalles => Set<ArmadoDetalle>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Producto>()
            .Property(p => p.Categoria)
            .HasConversion<string>();

        modelBuilder.Entity<ArmadoDetalle>()
            .HasOne(d => d.Armado)
            .WithMany(a => a.Detalles)
            .HasForeignKey(d => d.ArmadoId);

        modelBuilder.Entity<ArmadoDetalle>()
            .HasOne(d => d.Producto)
            .WithMany()
            .HasForeignKey(d => d.ProductoId);
    }
}
