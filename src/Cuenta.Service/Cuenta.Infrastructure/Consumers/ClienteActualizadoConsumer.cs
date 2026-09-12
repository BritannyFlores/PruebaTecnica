using Cuenta.Infrastructure.Persistence;
using MassTransit;
using Shared.Contracts.Events;

namespace Cuenta.Infrastructure.Consumers;

public class ClienteActualizadoConsumer : IConsumer<ClienteActualizado>
{
    private readonly CuentaDbContext _context;

    public ClienteActualizadoConsumer(CuentaDbContext context)
    {
        _context = context;
    }

    public async Task Consume(ConsumeContext<ClienteActualizado> context)
    {
        var evento = context.Message;

        var clienteRef = await _context.ClientesReferencia.FindAsync(evento.ClienteId);
        if (clienteRef is not null)
        {
            clienteRef.Nombre = evento.Nombre;
            clienteRef.Estado = evento.Estado;
            await _context.SaveChangesAsync();
        }
    }
}