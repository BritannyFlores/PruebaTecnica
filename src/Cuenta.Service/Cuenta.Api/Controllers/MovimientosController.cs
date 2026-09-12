using Cuenta.Application.DTOs;
using Cuenta.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Cuenta.Api.Controllers;

[ApiController]
[Route("movimientos")]
public class MovimientosController : ControllerBase
{
    private readonly IMovimientoService _movimientoService;

    public MovimientosController(IMovimientoService movimientoService)
    {
        _movimientoService = movimientoService;
    }

    [HttpGet("{numeroCuenta}")]
    public async Task<ActionResult<IEnumerable<MovimientoDto>>> ObtenerPorCuenta(string numeroCuenta)
    {
        var movimientos = await _movimientoService.ObtenerPorCuentaAsync(numeroCuenta);
        return Ok(movimientos);
    }

    [HttpPost]
    public async Task<ActionResult<MovimientoDto>> Crear([FromBody] CrearMovimientoDto dto)
    {
        var movimiento = await _movimientoService.CrearAsync(dto);
        return CreatedAtAction(nameof(ObtenerPorCuenta), new { numeroCuenta = movimiento.NumeroCuenta }, movimiento);
    }
}