using FluentAssertions;
using Xunit;

namespace Cliente.Tests.Entities;

public class ClienteTests
{
    [Fact]
    public void Cliente_DebeHeredarPropiedadesDePersona()
    {
        // Arrange & Act
        var cliente = new Domain.Entities.Cliente
        {
            Nombre = "Jose Lema",
            Genero = "Masculino",
            Edad = 35,
            Identificacion = "1234567890",
            Direccion = "Otavalo sn y principal",
            Telefono = "098254785",
            Contrasena = "1234",
            Estado = true
        };

        // Assert
        cliente.Nombre.Should().Be("Jose Lema");
        cliente.Identificacion.Should().Be("1234567890");
        cliente.Estado.Should().BeTrue();
    }

    [Fact]
    public void Cliente_EstadoPorDefecto_DebeSerActivo()
    {
        // Arrange & Act
        var cliente = new Domain.Entities.Cliente
        {
            Nombre = "Juan Osorio",
            Identificacion = "0987654321"
        };

        // Assert
        cliente.Estado.Should().BeTrue();
    }

    [Fact]
    public void Cliente_AlCrear_DebeAsignarseCorrectamenteContrasena()
    {
        // Arrange
        var contrasenaEsperada = "1234";

        // Act
        var cliente = new Domain.Entities.Cliente
        {
            Nombre = "Jose Lema",
            Identificacion = "1234567890",
            Contrasena = contrasenaEsperada
        };

        // Assert
        cliente.Contrasena.Should().Be(contrasenaEsperada);
    }
}