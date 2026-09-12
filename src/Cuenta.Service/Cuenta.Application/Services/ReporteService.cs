using Cuenta.Application.DTOs;
using Cuenta.Application.Exceptions;
using Cuenta.Application.Interfaces;

namespace Cuenta.Application.Services;

public class ReporteService : IReporteService
{
    private readonly ICuentaRepository _cuentaRepository;
    private readonly IMovimientoRepository _movimientoRepository;

    public ReporteService(ICuentaRepository cuentaRepository, IMovimientoRepository movimientoRepository)
    {
        _cuentaRepository = cuentaRepository;
        _movimientoRepository = movimientoRepository;
    }

    public async Task<ReporteEstadoCuentaDto> GenerarEstadoCuentaAsync(DateTime desde, DateTime hasta, int clienteId)
    {
        var existeCliente = await _cuentaRepository.ExisteClienteAsync(clienteId);
        if (!existeCliente)
        {
            throw new NotFoundException($"No se encontró el cliente con Id {clienteId}");
        }

        var cuentas = await _cuentaRepository.ObtenerPorClienteAsync(clienteId);

        var clienteRef = await _cuentaRepository.ObtenerClienteReferenciaAsync(clienteId);

        var reporte = new ReporteEstadoCuentaDto
        {
            ClienteId = clienteId,
            Cliente = clienteRef?.Nombre ?? string.Empty
        };

        foreach (var cuenta in cuentas)
        {
            var movimientos = await _movimientoRepository.ObtenerPorRangoFechasAsync(desde, hasta, clienteId);
            var movimientosDeCuenta = movimientos.Where(m => m.NumeroCuenta == cuenta.NumeroCuenta).ToList();

            var saldoDisponible = movimientosDeCuenta.Any()
                ? movimientosDeCuenta.Last().Saldo
                : cuenta.SaldoInicial;

            reporte.Cuentas.Add(new ReporteCuentaDto
            {
                NumeroCuenta = cuenta.NumeroCuenta,
                Tipo = cuenta.TipoCuenta,
                SaldoInicial = cuenta.SaldoInicial,
                Estado = cuenta.Estado,
                SaldoDisponible = saldoDisponible,
                Movimientos = movimientosDeCuenta.Select(m => new ReporteMovimientoDto
                {
                    Fecha = m.Fecha,
                    TipoMovimiento = m.TipoMovimiento,
                    Valor = m.Valor,
                    Saldo = m.Saldo
                }).ToList()
            });
        }

        return reporte;
    }
}