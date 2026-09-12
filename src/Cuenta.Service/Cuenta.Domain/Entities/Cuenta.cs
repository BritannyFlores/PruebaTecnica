namespace Cuenta.Domain.Entities;

public class Cuenta
{
    public string NumeroCuenta { get; set; } = string.Empty;
    public string TipoCuenta { get; set; } = string.Empty;
    public decimal SaldoInicial { get; set; }
    public bool Estado { get; set; } = true;

    public int ClienteId { get; set; }
    public ClienteReferencia? Cliente { get; set; }

    public ICollection<Movimiento> Movimientos { get; set; } = new List<Movimiento>();
}