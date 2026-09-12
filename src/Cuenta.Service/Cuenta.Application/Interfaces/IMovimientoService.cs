using Cuenta.Application.DTOs;

namespace Cuenta.Application.Interfaces;

public interface IMovimientoService
{
    Task<IEnumerable<MovimientoDto>> ObtenerPorCuentaAsync(string numeroCuenta);
    Task<MovimientoDto> CrearAsync(CrearMovimientoDto dto);
}