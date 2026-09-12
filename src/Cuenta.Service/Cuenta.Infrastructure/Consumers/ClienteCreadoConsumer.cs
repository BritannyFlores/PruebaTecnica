using Cuenta.Infrastructure.Persistence;
using MassTransit;
using Shared.Contracts.Events;

namespace Cuenta.Infrastructure.Consumers;

public class ClienteCreadoConsumer : IConsumer<ClienteCreado>
{
    private readonly CuentaDbContext _context;

    public ClienteCreadoConsumer(CuentaDbContext context)
    {
        _context = context;
    }

    public async Task Consume(ConsumeContext<ClienteCreado> context)
    {
        var evento = context.Message;

        var existe = await _context.ClientesReferencia.FindAsync(evento.ClienteId);
        if (existe is null)
        {
            _context.ClientesReferencia.Add(new Domain.Entities.ClienteReferencia
            {
                ClienteId = evento.ClienteId,
                Nombre = evento.Nombre,
                Estado = evento.Estado
            });

            await _context.SaveChangesAsync();
        }
    }
}