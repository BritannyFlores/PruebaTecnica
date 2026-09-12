using Cuenta.Application.DTOs;
using Cuenta.Application.Exceptions;
using Cuenta.Application.Interfaces;

namespace Cuenta.Application.Services;

public class CuentaService : ICuentaService
{
    private readonly ICuentaRepository _repository;

    public CuentaService(ICuentaRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CuentaDto>> ObtenerTodasAsync()
    {
        var cuentas = await _repository.ObtenerTodasAsync();
        return cuentas.Select(MapearADto);
    }

    public async Task<CuentaDto> ObtenerPorNumeroAsync(string numeroCuenta)
    {
        var cuenta = await _repository.ObtenerPorNumeroAsync(numeroCuenta)
            ?? throw new NotFoundException($"No se encontró la cuenta {numeroCuenta}");

        return MapearADto(cuenta);
    }

    public async Task<CuentaDto> CrearAsync(CrearCuentaDto dto)
    {
        var existeCuenta = await _repository.ExisteNumeroCuentaAsync(dto.NumeroCuenta);
        if (existeCuenta)
        {
            throw new BusinessRuleException($"Ya existe una cuenta con el número {dto.NumeroCuenta}");
        }

        var existeCliente = await _repository.ExisteClienteAsync(dto.ClienteId);
        if (!existeCliente)
        {
            throw new BusinessRuleException($"No existe un cliente con Id {dto.ClienteId}");
        }

        var cuenta = new Domain.Entities.Cuenta
        {
            NumeroCuenta = dto.NumeroCuenta,
            TipoCuenta = dto.TipoCuenta,
            SaldoInicial = dto.SaldoInicial,
            ClienteId = dto.ClienteId,
            Estado = true
        };

        var creada = await _repository.CrearAsync(cuenta);
        return MapearADto(creada);
    }

    public async Task ActualizarAsync(string numeroCuenta, ActualizarCuentaDto dto)
    {
        var cuenta = await _repository.ObtenerPorNumeroAsync(numeroCuenta)
            ?? throw new NotFoundException($"No se encontró la cuenta {numeroCuenta}");

        cuenta.TipoCuenta = dto.TipoCuenta;
        cuenta.Estado = dto.Estado;

        await _repository.ActualizarAsync(cuenta);
    }

    private static CuentaDto MapearADto(Domain.Entities.Cuenta cuenta)
    {
        return new CuentaDto
        {
            NumeroCuenta = cuenta.NumeroCuenta,
            TipoCuenta = cuenta.TipoCuenta,
            SaldoInicial = cuenta.SaldoInicial,
            Estado = cuenta.Estado,
            ClienteId = cuenta.ClienteId
        };
    }
}