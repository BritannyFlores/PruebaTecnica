using Cuenta.Api.Middleware;
using Cuenta.Application.Interfaces;
using Cuenta.Application.Services;
using Cuenta.Infrastructure.Persistence;
using Cuenta.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

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
