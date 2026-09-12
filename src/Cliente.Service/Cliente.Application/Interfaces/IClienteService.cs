using Cliente.Application.DTOs;

namespace Cliente.Application.Interfaces;

public interface IClienteService
{
    Task<IEnumerable<ClienteDto>> ObtenerTodosAsync();
    Task<ClienteDto> ObtenerPorIdAsync(int clienteId);
    Task<ClienteDto> CrearAsync(CrearClienteDto dto);
    Task ActualizarAsync(int clienteId, ActualizarClienteDto dto);
    Task EliminarAsync(int clienteId);
}