using Cuenta.Application.DTOs;

namespace Cuenta.Application.Interfaces;

public interface IReporteService
{
    Task<ReporteEstadoCuentaDto> GenerarEstadoCuentaAsync(DateTime desde, DateTime hasta, int clienteId);
}