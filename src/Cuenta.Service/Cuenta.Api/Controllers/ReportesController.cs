using Cuenta.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Cuenta.Api.Controllers;

[ApiController]
[Route("reportes")]
public class ReportesController : ControllerBase
{
    private readonly IReporteService _reporteService;

    public ReportesController(IReporteService reporteService)
    {
        _reporteService = reporteService;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerEstadoCuenta(
        [FromQuery] DateTime fechaInicio,
        [FromQuery] DateTime fechaFin,
        [FromQuery] int cliente)
    {
        var reporte = await _reporteService.GenerarEstadoCuentaAsync(fechaInicio, fechaFin, cliente);
        return Ok(reporte);
    }
}