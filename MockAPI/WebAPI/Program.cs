using Infrastructure;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Service;
using System.Text.Json;
using WebAPI.middlewares;

var builder = WebApplication.CreateBuilder(args);

//Env variabels
var ConnectionString = "Server=localhost;Database=Endpoints;User Id=sa;Password=pvg@zeq4RWQ3wxr-rhn;Trusted_Connection=False;TrustServerCertificate=True;";
var SqlDatabaseType ="mssql";
var DevMode = "false";

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

switch (SqlDatabaseType)
{
    case "postgres":
        builder.Services.AddDbContext<DatabaseContext>(options =>
            options.UseNpgsql(ConnectionString));
        break;

    case "mssql":
        builder.Services.AddDbContext<DatabaseContext>(options =>
            options.UseSqlServer(ConnectionString, sqlOptions => sqlOptions.MigrationsAssembly("WebAPI")));
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

app.UseCors("AllowAll");

app.UseMiddleware<DynamicMockMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();