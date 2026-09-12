namespace Cuenta.Application.Interfaces;

public interface ICuentaRepository
{
    Task<IEnumerable<Domain.Entities.Cuenta>> ObtenerTodasAsync();
    Task<Domain.Entities.Cuenta?> ObtenerPorNumeroAsync(string numeroCuenta);
    Task<Domain.Entities.Cuenta> CrearAsync(Domain.Entities.Cuenta cuenta);
    Task ActualizarAsync(Domain.Entities.Cuenta cuenta);
    Task<bool> ExisteNumeroCuentaAsync(string numeroCuenta);
    Task<bool> ExisteClienteAsync(int clienteId);
    Task<IEnumerable<Domain.Entities.Cuenta>> ObtenerPorClienteAsync(int clienteId);
    Task<Domain.Entities.ClienteReferencia?> ObtenerClienteReferenciaAsync(int clienteId);

}