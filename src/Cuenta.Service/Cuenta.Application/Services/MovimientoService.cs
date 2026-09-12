using Cuenta.Application.DTOs;
using Cuenta.Application.Exceptions;
using Cuenta.Application.Interfaces;

namespace Cuenta.Application.Services;

public class MovimientoService : IMovimientoService
{
    private readonly IMovimientoRepository _movimientoRepository;
    private readonly ICuentaRepository _cuentaRepository;

    public MovimientoService(IMovimientoRepository movimientoRepository, ICuentaRepository cuentaRepository)
    {
        _movimientoRepository = movimientoRepository;
        _cuentaRepository = cuentaRepository;
    }

    public async Task<IEnumerable<MovimientoDto>> ObtenerPorCuentaAsync(string numeroCuenta)
    {
        var movimientos = await _movimientoRepository.ObtenerPorCuentaAsync(numeroCuenta);
        return movimientos.Select(MapearADto);
    }

    public async Task<MovimientoDto> CrearAsync(CrearMovimientoDto dto)
    {
        var cuenta = await _cuentaRepository.ObtenerPorNumeroAsync(dto.NumeroCuenta)
            ?? throw new NotFoundException($"No se encontró la cuenta {dto.NumeroCuenta}");

        // F2: calcular el nuevo saldo a partir del último movimiento (o el saldo inicial si es el primero)
        var movimientosPrevios = await _movimientoRepository.ObtenerPorCuentaAsync(dto.NumeroCuenta);
        var saldoActual = movimientosPrevios.Any()
            ? movimientosPrevios.Last().Saldo
            : cuenta.SaldoInicial;

        var nuevoSaldo = saldoActual + dto.Valor;

        // F3: validar saldo disponible antes de permitir un retiro
        if (nuevoSaldo < 0)
        {
            throw new BusinessRuleException("Saldo no disponible");
        }

        var movimiento = new Domain.Entities.Movimiento
        {
            Fecha = DateTime.UtcNow,
            TipoMovimiento = dto.TipoMovimiento,
            Valor = dto.Valor,
            Saldo = nuevoSaldo,
            NumeroCuenta = dto.NumeroCuenta
        };

        var creado = await _movimientoRepository.CrearAsync(movimiento);
        return MapearADto(creado);
    }

    private static MovimientoDto MapearADto(Domain.Entities.Movimiento movimiento)
    {
        return new MovimientoDto
        {
            MovimientoId = movimiento.MovimientoId,
            Fecha = movimiento.Fecha,
            TipoMovimiento = movimiento.TipoMovimiento,
            Valor = movimiento.Valor,
            Saldo = movimiento.Saldo,
            NumeroCuenta = movimiento.NumeroCuenta
        };
    }
}