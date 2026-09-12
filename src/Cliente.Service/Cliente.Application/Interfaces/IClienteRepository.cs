namespace Cliente.Application.Interfaces;

public interface IClienteRepository
{
    Task<IEnumerable<Domain.Entities.Cliente>> ObtenerTodosAsync();
    Task<Domain.Entities.Cliente?> ObtenerPorIdAsync(int clienteId);
    Task<Domain.Entities.Cliente> CrearAsync(Domain.Entities.Cliente cliente);
    Task ActualizarAsync(Domain.Entities.Cliente cliente);
    Task EliminarAsync(int clienteId);
    Task<bool> ExisteIdentificacionAsync(string identificacion);
}