using System;
using System.Net;

namespace API.Middleware;

public class ExceptionMiddleware(RequestDelegate next, ILogger<Exception> logger, IHostEnvironment env)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{message}",ex.Message);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = env.IsDevelopment()
                ? new Errors.ApiExceptions(context.Response.StatusCode, ex.Message, ex.StackTrace)
                : new Errors.ApiExceptions(context.Response.StatusCode, "Internal Server Error", null);

            var options = new System.Text.Json.JsonSerializerOptions { PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase };

            var json = System.Text.Json.JsonSerializer.Serialize(response, options);

            await context.Response.WriteAsync(json);
        }
    }
}
