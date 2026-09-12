using Cuenta.Application.Interfaces;
using Cuenta.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Cuenta.Infrastructure.Repositories;

public class CuentaRepository : ICuentaRepository
{
    private readonly CuentaDbContext _context;

    public CuentaRepository(CuentaDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Domain.Entities.Cuenta>> ObtenerTodasAsync()
    {
        return await _context.Cuentas.ToListAsync();
    }

    public async Task<Domain.Entities.Cuenta?> ObtenerPorNumeroAsync(string numeroCuenta)
    {
        return await _context.Cuentas
            .FirstOrDefaultAsync(c => c.NumeroCuenta == numeroCuenta);
    }

    public async Task<Domain.Entities.Cuenta> CrearAsync(Domain.Entities.Cuenta cuenta)
    {
        _context.Cuentas.Add(cuenta);
        await _context.SaveChangesAsync();
        return cuenta;
    }

    public async Task ActualizarAsync(Domain.Entities.Cuenta cuenta)
    {
        _context.Cuentas.Update(cuenta);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExisteNumeroCuentaAsync(string numeroCuenta)
    {
        return await _context.Cuentas.AnyAsync(c => c.NumeroCuenta == numeroCuenta);
    }

    public async Task<bool> ExisteClienteAsync(int clienteId)
    {
        return await _context.ClientesReferencia.AnyAsync(c => c.ClienteId == clienteId);
    }
}