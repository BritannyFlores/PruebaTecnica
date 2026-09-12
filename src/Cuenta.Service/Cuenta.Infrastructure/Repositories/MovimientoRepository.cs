using Cuenta.Application.Interfaces;
using Cuenta.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Cuenta.Infrastructure.Repositories;

public class MovimientoRepository : IMovimientoRepository
{
    private readonly CuentaDbContext _context;

    public MovimientoRepository(CuentaDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Domain.Entities.Movimiento>> ObtenerPorCuentaAsync(string numeroCuenta)
    {
        return await _context.Movimientos
            .Where(m => m.NumeroCuenta == numeroCuenta)
            .OrderBy(m => m.Fecha)
            .ToListAsync();
    }

    public async Task<Domain.Entities.Movimiento> CrearAsync(Domain.Entities.Movimiento movimiento)
    {
        _context.Movimientos.Add(movimiento);
        await _context.SaveChangesAsync();
        return movimiento;
    }

    public async Task<IEnumerable<Domain.Entities.Movimiento>> ObtenerPorRangoFechasAsync(DateTime desde, DateTime hasta, int clienteId)
    {
        return await _context.Movimientos
            .Include(m => m.CuentaRef)
            .Where(m => m.CuentaRef!.ClienteId == clienteId && m.Fecha >= desde && m.Fecha <= hasta)
            .OrderBy(m => m.Fecha)
            .ToListAsync();
    }
}