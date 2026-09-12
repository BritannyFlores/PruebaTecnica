using Cliente.Application.Interfaces;
using Cliente.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Cliente.Infrastructure.Repositories;

public class ClienteRepository : IClienteRepository
{
    private readonly ClienteDbContext _context;

    public ClienteRepository(ClienteDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Domain.Entities.Cliente>> ObtenerTodosAsync()
    {
        return await _context.Clientes.ToListAsync();
    }

    public async Task<Domain.Entities.Cliente?> ObtenerPorIdAsync(int clienteId)
    {
        return await _context.Clientes
            .FirstOrDefaultAsync(c => c.ClienteId == clienteId);
    }

    public async Task<Domain.Entities.Cliente> CrearAsync(Domain.Entities.Cliente cliente)
    {
        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();
        return cliente;
    }

    public async Task ActualizarAsync(Domain.Entities.Cliente cliente)
    {
        _context.Clientes.Update(cliente);
        await _context.SaveChangesAsync();
    }

    public async Task EliminarAsync(int clienteId)
    {
        var cliente = await ObtenerPorIdAsync(clienteId);
        if (cliente is not null)
        {
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExisteIdentificacionAsync(string identificacion)
    {
        return await _context.Clientes.AnyAsync(c => c.Identificacion == identificacion);
    }
}