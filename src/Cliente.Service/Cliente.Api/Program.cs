using Cliente.Api.Middleware;
using Cliente.Application.Interfaces;
using Cliente.Application.Services;
using Cliente.Infrastructure.Persistence;
using Cliente.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ClienteDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ClienteConnection")));

builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IClienteService, ClienteService>();

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
