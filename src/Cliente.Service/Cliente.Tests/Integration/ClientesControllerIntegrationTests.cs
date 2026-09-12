using System.Net;
using System.Net.Http.Json;
using Cliente.Application.DTOs;
using Cliente.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Cliente.Tests.Integration;

public class ClientesControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ClientesControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        var factoryConfigurada = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ClienteDbContext>));
                if (descriptor is not null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<ClienteDbContext>(options =>
                    options.UseInMemoryDatabase("ClienteDbPruebas"));
            });
        });

        _client = factoryConfigurada.CreateClient();
    }

    [Fact]
    public async Task PostClientes_DebeCrearClienteYRetornar201()
    {
        // Arrange
        var nuevoCliente = new CrearClienteDto
        {
            Nombre = "Juan Osorio",
            Genero = "Masculino",
            Edad = 40,
            Identificacion = "1122334455",
            Direccion = "13 junio y Equinoccial",
            Telefono = "098874587",
            Contrasena = "1245"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/clientes", nuevoCliente);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var clienteCreado = await response.Content.ReadFromJsonAsync<ClienteDto>();
        clienteCreado.Should().NotBeNull();
        clienteCreado!.Nombre.Should().Be("Juan Osorio");
        clienteCreado.Identificacion.Should().Be("1122334455");
    }

    [Fact]
    public async Task GetClientePorId_CuandoNoExiste_DebeRetornar404()
    {
        // Act
        var response = await _client.GetAsync("/clientes/9999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}