namespace Shared.Contracts.Events;

public record ClienteActualizado(
    int ClienteId,
    string Nombre,
    bool Estado
);