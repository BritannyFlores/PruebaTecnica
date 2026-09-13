using Cuenta.Api.Middleware;
using Cuenta.Application.Interfaces;
using Cuenta.Application.Services;
using Cuenta.Infrastructure.Persistence;
using Cuenta.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Cuenta.Infrastructure.Consumers;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CuentaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CuentaConnection")));

builder.Services.AddScoped<ICuentaRepository, CuentaRepository>();
builder.Services.AddScoped<ICuentaService, CuentaService>();

builder.Services.AddScoped<IMovimientoRepository, MovimientoRepository>();
builder.Services.AddScoped<IMovimientoService, MovimientoService>();
builder.Services.AddScoped<IReporteService, ReporteService>();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<ClienteCreadoConsumer>();
    x.AddConsumer<ClienteActualizadoConsumer>();
    x.AddConsumer<ClienteEliminadoConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        var rabbitHost = builder.Configuration["RabbitMq:Host"] ?? "localhost";
        cfg.Host(rabbitHost, "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
