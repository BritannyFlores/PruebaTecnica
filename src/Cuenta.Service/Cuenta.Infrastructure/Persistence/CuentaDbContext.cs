using Microsoft.EntityFrameworkCore;

namespace Cuenta.Infrastructure.Persistence;

public class CuentaDbContext : DbContext
{
    public CuentaDbContext(DbContextOptions<CuentaDbContext> options) : base(options)
    {
    }

    public DbSet<Domain.Entities.ClienteReferencia> ClientesReferencia => Set<Domain.Entities.ClienteReferencia>();
    public DbSet<Domain.Entities.Cuenta> Cuentas => Set<Domain.Entities.Cuenta>();
    public DbSet<Domain.Entities.Movimiento> Movimientos => Set<Domain.Entities.Movimiento>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Domain.Entities.ClienteReferencia>().ToTable("ClientesReferencia");
        modelBuilder.Entity<Domain.Entities.ClienteReferencia>().HasKey(c => c.ClienteId);
        modelBuilder.Entity<Domain.Entities.ClienteReferencia>().Property(c => c.ClienteId).ValueGeneratedNever();

        modelBuilder.Entity<Domain.Entities.Cuenta>().ToTable("Cuentas");
        modelBuilder.Entity<Domain.Entities.Cuenta>().HasKey(c => c.NumeroCuenta);
        modelBuilder.Entity<Domain.Entities.Cuenta>()
            .HasOne(c => c.Cliente)
            .WithMany(cr => cr.Cuentas)
            .HasForeignKey(c => c.ClienteId);

        modelBuilder.Entity<Domain.Entities.Movimiento>().ToTable("Movimientos");
        modelBuilder.Entity<Domain.Entities.Movimiento>().HasKey(m => m.MovimientoId);
        modelBuilder.Entity<Domain.Entities.Movimiento>().Property(m => m.MovimientoId).ValueGeneratedOnAdd();
        modelBuilder.Entity<Domain.Entities.Movimiento>()
            .HasOne(m => m.CuentaRef)
            .WithMany(c => c.Movimientos)
            .HasForeignKey(m => m.NumeroCuenta);
    }
}