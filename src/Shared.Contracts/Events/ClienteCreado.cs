namespace Shared.Contracts.Events;

public record ClienteCreado(
    int ClienteId,
    string Nombre,
    bool Estado
);