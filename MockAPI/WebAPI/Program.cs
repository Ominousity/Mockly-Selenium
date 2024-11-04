using Infrastructure;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Service;
using System.Text.Json;
using WebAPI.middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<EndpointDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
                sqlOptions => sqlOptions.MigrationsAssembly("WebAPI")));
builder.Services.AddDbContext<ResponseObjectDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
                sqlOptions => sqlOptions.MigrationsAssembly("WebAPI")));

builder.Services.AddScoped<IResponseObjectInternalRepo, ResponseObjectInternalRepo>();
builder.Services.AddScoped<IEndpointInternalRepo, EndpointInternalRepo>();
builder.Services.AddScoped<IEndpointInternalService, EndpointInternalService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // Specify your frontend origin
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials(); // Only add this if you need credentials (like cookies or authorization headers)
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowLocalhost");

app.UseRouting();

app.UseMiddleware<DynamicMockMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();