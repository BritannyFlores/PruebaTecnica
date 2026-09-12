namespace Cuenta.Application.DTOs;

public class CrearCuentaDto
{
    public string NumeroCuenta { get; set; } = string.Empty;
    public string TipoCuenta { get; set; } = string.Empty;
    public decimal SaldoInicial { get; set; }
    public int ClienteId { get; set; }
}