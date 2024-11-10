using Infrastructure.Interfaces;
using Infrastructure.Enums;
using HttpMethods = Infrastructure.Enums.HttpMethods;
using Service;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace WebAPI.middlewares
{
    public class DynamicMockMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceProvider _serviceProvider;

        public DynamicMockMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
        {
            _next = next;
            _serviceProvider = serviceProvider;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var endpointRepo = scope.ServiceProvider.GetRequiredService<IEndpointInternalService>();
                var path = context.Request.Path.Value.ToLower();
                var method = context.Request.Method.ToUpper();

                HttpMethods httpMethod = method switch
                {
                    "GET" => HttpMethods.Get,
                    "POST" => HttpMethods.Post,
                    "PUT" => HttpMethods.Put,
                    "DELETE" => HttpMethods.Delete,
                    _ => throw new NotImplementedException("Method not implemented.")
                };

                // Find matching endpoint in your repository (e.g., database)
                try
                {
                    var mockEndpoint = await endpointRepo.GetEndpointAsync(path, httpMethod);

                    if (mockEndpoint != null)
                    {
                        // Set response headers
                        context.Response.StatusCode = 200;

                        // Write the mock response
                        if (mockEndpoint.ResponseObject != null)
                        {
                            await context.Response.WriteAsync(JsonSerializer.Serialize(mockEndpoint.ResponseObject));
                        }
                        else
                        {
                            await context.Response.WriteAsync("");
                        }
                    }
                    else
                    {
                        // Pass to the next middleware if no match found
                        await _next(context);
                    }
                }
                catch
                {
                    await _next(context);
                }
            }
        }
    }
}
