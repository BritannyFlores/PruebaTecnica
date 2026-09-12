using Cuenta.Infrastructure.Persistence;
using MassTransit;
using Shared.Contracts.Events;

namespace Cuenta.Infrastructure.Consumers;

public class ClienteEliminadoConsumer : IConsumer<ClienteEliminado>
{
    private readonly CuentaDbContext _context;

    public ClienteEliminadoConsumer(CuentaDbContext context)
    {
        _context = context;
    }

    public async Task Consume(ConsumeContext<ClienteEliminado> context)
    {
        var evento = context.Message;

        var clienteRef = await _context.ClientesReferencia.FindAsync(evento.ClienteId);
        if (clienteRef is not null)
        {
            _context.ClientesReferencia.Remove(clienteRef);
            await _context.SaveChangesAsync();
        }
    }
}