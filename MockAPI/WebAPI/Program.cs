using Infrastructure;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Service;
using System.Text.Json;
using WebAPI.middlewares;

var builder = WebApplication.CreateBuilder(args);

//Env variabels
var ConnectionString = Environment.GetEnvironmentVariable("cs") ?? "Server=10.40.143.113;Database=Endpoints;User Id=sa;Password=pvg@zeq4RWQ3wxr-rhn;Trusted_Connection=False;TrustServerCertificate=True;";
var SqlDatabaseType = Environment.GetEnvironmentVariable("type") ?? "mssql";
var DevMode = Environment.GetEnvironmentVariable("dev") ?? "false";

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

switch (SqlDatabaseType)
{
    case "postgres":
        builder.Services.AddDbContext<EndpointDbContext>(options =>
            options.UseNpgsql(ConnectionString));

        builder.Services.AddDbContext<ResponseObjectDbContext>(options =>
            options.UseNpgsql(ConnectionString));
        break;

    case "mssql":
        builder.Services.AddDbContext<EndpointDbContext>(options =>
            options.UseSqlServer(ConnectionString, sqlOptions => sqlOptions.MigrationsAssembly("WebAPI")));

        builder.Services.AddDbContext<ResponseObjectDbContext>(options =>
            options.UseSqlServer(ConnectionString));
        break;
}


builder.Services.AddScoped<IResponseObjectInternalRepo, ResponseObjectInternalRepo>();
builder.Services.AddScoped<IEndpointInternalRepo, EndpointInternalRepo>();
builder.Services.AddScoped<IEndpointInternalService, EndpointInternalService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseMiddleware<DynamicMockMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();