using Cliente.Application.DTOs;
using Cliente.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Cliente.Api.Controllers;

[ApiController]
[Route("clientes")]
public class ClientesController : ControllerBase
{
    private readonly IClienteService _clienteService;

    public ClientesController(IClienteService clienteService)
    {
        _clienteService = clienteService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClienteDto>>> ObtenerTodos()
    {
        var clientes = await _clienteService.ObtenerTodosAsync();
        return Ok(clientes);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ClienteDto>> ObtenerPorId(int id)
    {
        var cliente = await _clienteService.ObtenerPorIdAsync(id);
        return Ok(cliente);
    }

    [HttpPost]
    public async Task<ActionResult<ClienteDto>> Crear([FromBody] CrearClienteDto dto)
    {
        var cliente = await _clienteService.CrearAsync(dto);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = cliente.ClienteId }, cliente);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarClienteDto dto)
    {
        await _clienteService.ActualizarAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        await _clienteService.EliminarAsync(id);
        return NoContent();
    }
}