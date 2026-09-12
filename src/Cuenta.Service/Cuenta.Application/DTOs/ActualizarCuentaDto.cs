namespace Cuenta.Application.DTOs;

public class ActualizarCuentaDto
{
    public string TipoCuenta { get; set; } = string.Empty;
    public bool Estado { get; set; }
}