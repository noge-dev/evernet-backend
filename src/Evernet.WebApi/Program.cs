using Evernet.WebApi.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<EvernetDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("postgresdb")
                      ?? throw new InvalidOperationException("Connection string 'postgresdb' not found.")));

// builder.Services.AddDbContext<EvernetDbContext>(options =>
//     options.UseNpgsql(builder.Configuration.GetConnectionString("defaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();