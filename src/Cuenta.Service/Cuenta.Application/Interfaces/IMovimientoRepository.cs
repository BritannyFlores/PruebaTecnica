namespace Cuenta.Application.Interfaces;

public interface IMovimientoRepository
{
    Task<IEnumerable<Domain.Entities.Movimiento>> ObtenerPorCuentaAsync(string numeroCuenta);
    Task<Domain.Entities.Movimiento> CrearAsync(Domain.Entities.Movimiento movimiento);
    Task<IEnumerable<Domain.Entities.Movimiento>> ObtenerPorRangoFechasAsync(DateTime desde, DateTime hasta, int clienteId);
}