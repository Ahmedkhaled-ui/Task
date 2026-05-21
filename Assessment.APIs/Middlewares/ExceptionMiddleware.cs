using Application.DTOS.Api;
using System.Net;
using System.Text.Json;

namespace Assessment.APIs.Middlewares
{
    public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);

                if (context.Response.StatusCode == StatusCodes.Status404NotFound && !context.Response.HasStarted)
                {
                    context.Response.ContentType = "application/json";

                    var response = ApiResponse<object>.FailureResponse($"Endpoint '{context.Request.Path}' not found.");
                    var json = JsonSerializer.Serialize(response, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

                    await context.Response.WriteAsync(json);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex, env);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception, IHostEnvironment env)
        {
            context.Response.ContentType = "application/json";

            context.Response.StatusCode = exception switch
            {
                ArgumentException => (int)HttpStatusCode.BadRequest,            
                InvalidOperationException => (int)HttpStatusCode.BadRequest,     
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,  
                KeyNotFoundException => (int)HttpStatusCode.NotFound,             
                _ => (int)HttpStatusCode.InternalServerError                      
            };

            string message = exception.Message;

            if (env.IsDevelopment() && context.Response.StatusCode == (int)HttpStatusCode.InternalServerError)
            {
                message = $"{exception.Message} | Details: {exception.StackTrace}";
            }

            var response = ApiResponse<object>.FailureResponse(message);
            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            return context.Response.WriteAsync(json);
        }
    }

    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}