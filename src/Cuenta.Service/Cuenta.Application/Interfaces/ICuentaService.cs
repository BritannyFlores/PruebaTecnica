using Cuenta.Application.DTOs;

namespace Cuenta.Application.Interfaces;

public interface ICuentaService
{
    Task<IEnumerable<CuentaDto>> ObtenerTodasAsync();
    Task<CuentaDto> ObtenerPorNumeroAsync(string numeroCuenta);
    Task<CuentaDto> CrearAsync(CrearCuentaDto dto);
    Task ActualizarAsync(string numeroCuenta, ActualizarCuentaDto dto);
}