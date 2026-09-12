namespace Cuenta.Application.DTOs;

public class ReporteEstadoCuentaDto
{
    public int ClienteId { get; set; }
    public string Cliente { get; set; } = string.Empty;
    public List<ReporteCuentaDto> Cuentas { get; set; } = new();
}

public class ReporteCuentaDto
{
    public string NumeroCuenta { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public decimal SaldoInicial { get; set; }
    public bool Estado { get; set; }
    public decimal SaldoDisponible { get; set; }
    public List<ReporteMovimientoDto> Movimientos { get; set; } = new();
}

public class ReporteMovimientoDto
{
    public DateTime Fecha { get; set; }
    public string TipoMovimiento { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public decimal Saldo { get; set; }
}