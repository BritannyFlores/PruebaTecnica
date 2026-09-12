using Microsoft.EntityFrameworkCore;

namespace Cliente.Infrastructure.Persistence;

public class ClienteDbContext : DbContext
{
    public ClienteDbContext(DbContextOptions<ClienteDbContext> options) : base(options)
    {
    }

    public DbSet<Domain.Entities.Persona> Personas => Set<Domain.Entities.Persona>();
    public DbSet<Domain.Entities.Cliente> Clientes => Set<Domain.Entities.Cliente>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Domain.Entities.Persona>().ToTable("Personas");
        modelBuilder.Entity<Domain.Entities.Persona>().HasKey(p => p.PersonaId);
        modelBuilder.Entity<Domain.Entities.Persona>().Property(p => p.PersonaId).ValueGeneratedOnAdd();
        modelBuilder.Entity<Domain.Entities.Persona>().HasIndex(p => p.Identificacion).IsUnique();

        modelBuilder.Entity<Domain.Entities.Cliente>().ToTable("Clientes");
        modelBuilder.Entity<Domain.Entities.Cliente>().Property(c => c.ClienteId).ValueGeneratedOnAdd();
        modelBuilder.Entity<Domain.Entities.Cliente>().HasIndex(c => c.ClienteId).IsUnique();
    }
}