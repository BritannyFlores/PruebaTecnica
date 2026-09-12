namespace Cuenta.Domain.Entities;

public class ClienteReferencia
{
    public int ClienteId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public bool Estado { get; set; } = true;

    public ICollection<Cuenta> Cuentas { get; set; } = new List<Cuenta>();
}