-- Prueba Tecnica - Base de Datos
-- BASE DE DATOS: ClienteDB
CREATE DATABASE ClienteDB;
GO
USE ClienteDB;
GO

IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Personas] (
    [PersonaId] int NOT NULL IDENTITY,
    [Nombre] nvarchar(max) NOT NULL,
    [Genero] nvarchar(max) NOT NULL,
    [Edad] int NOT NULL,
    [Identificacion] nvarchar(450) NOT NULL,
    [Direccion] nvarchar(max) NOT NULL,
    [Telefono] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Personas] PRIMARY KEY ([PersonaId])
);
GO

CREATE TABLE [Clientes] (
    [PersonaId] int NOT NULL,
    [ClienteId] int NOT NULL IDENTITY,
    [Contrasena] nvarchar(max) NOT NULL,
    [Estado] bit NOT NULL,
    CONSTRAINT [PK_Clientes] PRIMARY KEY ([PersonaId]),
    CONSTRAINT [FK_Clientes_Personas_PersonaId] FOREIGN KEY ([PersonaId]) REFERENCES [Personas] ([PersonaId]) ON DELETE CASCADE
);
GO

CREATE UNIQUE INDEX [IX_Clientes_ClienteId] ON [Clientes] ([ClienteId]) WHERE [ClienteId] IS NOT NULL;
GO

CREATE UNIQUE INDEX [IX_Personas_Identificacion] ON [Personas] ([Identificacion]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260910031358_InitialCreate', N'8.0.11');
GO

COMMIT;
GO

-- BASE DE DATOS: CuentaDB
CREATE DATABASE CuentaDB;
GO
USE CuentaDB;
GO

IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [ClientesReferencia] (
    [ClienteId] int NOT NULL,
    [Nombre] nvarchar(max) NOT NULL,
    [Estado] bit NOT NULL,
    CONSTRAINT [PK_ClientesReferencia] PRIMARY KEY ([ClienteId])
);
GO

CREATE TABLE [Cuentas] (
    [NumeroCuenta] nvarchar(450) NOT NULL,
    [TipoCuenta] nvarchar(max) NOT NULL,
    [SaldoInicial] decimal(18,2) NOT NULL,
    [Estado] bit NOT NULL,
    [ClienteId] int NOT NULL,
    CONSTRAINT [PK_Cuentas] PRIMARY KEY ([NumeroCuenta]),
    CONSTRAINT [FK_Cuentas_ClientesReferencia_ClienteId] FOREIGN KEY ([ClienteId]) REFERENCES [ClientesReferencia] ([ClienteId]) ON DELETE CASCADE
);
GO

CREATE TABLE [Movimientos] (
    [MovimientoId] int NOT NULL IDENTITY,
    [Fecha] datetime2 NOT NULL,
    [TipoMovimiento] nvarchar(max) NOT NULL,
    [Valor] decimal(18,2) NOT NULL,
    [Saldo] decimal(18,2) NOT NULL,
    [NumeroCuenta] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_Movimientos] PRIMARY KEY ([MovimientoId]),
    CONSTRAINT [FK_Movimientos_Cuentas_NumeroCuenta] FOREIGN KEY ([NumeroCuenta]) REFERENCES [Cuentas] ([NumeroCuenta]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_Cuentas_ClienteId] ON [Cuentas] ([ClienteId]);
GO

CREATE INDEX [IX_Movimientos_NumeroCuenta] ON [Movimientos] ([NumeroCuenta]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260910033646_InitialCreate', N'8.0.11');
GO

COMMIT;
GO

