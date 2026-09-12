using Cliente.Application.DTOs;
using Cliente.Application.Exceptions;
using Cliente.Application.Interfaces;
using MassTransit;
using Shared.Contracts.Events;

namespace Cliente.Application.Services;

public class ClienteService : IClienteService
{
    private readonly IClienteRepository _repository;
    private readonly IPublishEndpoint _publishEndpoint;

    public ClienteService(IClienteRepository repository, IPublishEndpoint publishEndpoint)
    {
        _repository = repository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<IEnumerable<ClienteDto>> ObtenerTodosAsync()
    {
        var clientes = await _repository.ObtenerTodosAsync();
        return clientes.Select(MapearADto);
    }

    public async Task<ClienteDto> ObtenerPorIdAsync(int clienteId)
    {
        var cliente = await _repository.ObtenerPorIdAsync(clienteId)
            ?? throw new NotFoundException($"No se encontró el cliente con Id {clienteId}");

        return MapearADto(cliente);
    }

    public async Task<ClienteDto> CrearAsync(CrearClienteDto dto)
    {
        var existeIdentificacion = await _repository.ExisteIdentificacionAsync(dto.Identificacion);
        if (existeIdentificacion)
        {
            throw new BusinessRuleException($"Ya existe un cliente con la identificación {dto.Identificacion}");
        }

        var cliente = new Domain.Entities.Cliente
        {
            Nombre = dto.Nombre,
            Genero = dto.Genero,
            Edad = dto.Edad,
            Identificacion = dto.Identificacion,
            Direccion = dto.Direccion,
            Telefono = dto.Telefono,
            Contrasena = dto.Contrasena,
            Estado = true
        };

        var creado = await _repository.CrearAsync(cliente);

        await _publishEndpoint.Publish(new ClienteCreado(creado.ClienteId, creado.Nombre, creado.Estado));

        return MapearADto(creado);
    }

    public async Task ActualizarAsync(int clienteId, ActualizarClienteDto dto)
    {
        var cliente = await _repository.ObtenerPorIdAsync(clienteId)
            ?? throw new NotFoundException($"No se encontró el cliente con Id {clienteId}");

        cliente.Nombre = dto.Nombre;
        cliente.Genero = dto.Genero;
        cliente.Edad = dto.Edad;
        cliente.Direccion = dto.Direccion;
        cliente.Telefono = dto.Telefono;
        cliente.Estado = dto.Estado;

        await _repository.ActualizarAsync(cliente);

        await _publishEndpoint.Publish(new ClienteActualizado(cliente.ClienteId, cliente.Nombre, cliente.Estado));
    }

    public async Task EliminarAsync(int clienteId)
    {
        var cliente = await _repository.ObtenerPorIdAsync(clienteId)
            ?? throw new NotFoundException($"No se encontró el cliente con Id {clienteId}");

        await _repository.EliminarAsync(clienteId);

        await _publishEndpoint.Publish(new ClienteEliminado(clienteId));
    }

    private static ClienteDto MapearADto(Domain.Entities.Cliente cliente)
    {
        return new ClienteDto
        {
            ClienteId = cliente.ClienteId,
            Nombre = cliente.Nombre,
            Genero = cliente.Genero,
            Edad = cliente.Edad,
            Identificacion = cliente.Identificacion,
            Direccion = cliente.Direccion,
            Telefono = cliente.Telefono,
            Estado = cliente.Estado
        };
    }
}