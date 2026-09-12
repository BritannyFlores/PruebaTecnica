using Cuenta.Application.DTOs;
using Cuenta.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Cuenta.Api.Controllers;

[ApiController]
[Route("cuentas")]
public class CuentasController : ControllerBase
{
    private readonly ICuentaService _cuentaService;

    public CuentasController(ICuentaService cuentaService)
    {
        _cuentaService = cuentaService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CuentaDto>>> ObtenerTodas()
    {
        var cuentas = await _cuentaService.ObtenerTodasAsync();
        return Ok(cuentas);
    }

    [HttpGet("{numeroCuenta}")]
    public async Task<ActionResult<CuentaDto>> ObtenerPorNumero(string numeroCuenta)
    {
        var cuenta = await _cuentaService.ObtenerPorNumeroAsync(numeroCuenta);
        return Ok(cuenta);
    }

    [HttpPost]
    public async Task<ActionResult<CuentaDto>> Crear([FromBody] CrearCuentaDto dto)
    {
        var cuenta = await _cuentaService.CrearAsync(dto);
        return CreatedAtAction(nameof(ObtenerPorNumero), new { numeroCuenta = cuenta.NumeroCuenta }, cuenta);
    }

    [HttpPut("{numeroCuenta}")]
    public async Task<IActionResult> Actualizar(string numeroCuenta, [FromBody] ActualizarCuentaDto dto)
    {
        await _cuentaService.ActualizarAsync(numeroCuenta, dto);
        return NoContent();
    }
}