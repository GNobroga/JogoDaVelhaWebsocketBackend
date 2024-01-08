using System.ComponentModel.DataAnnotations;

namespace JogoVelha.Application.Middlewares;

public class GlobalExceptionHandlerMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try 
        {
            await next(context);
        }
        catch (ArgumentException ex) 
        {
            await HandleExceptionAsync(context, "Argument invalid", 400, ex.Message);
        }
        catch 
        {
            await HandleExceptionAsync(context, "Server error", 500, "A server error occurred.");
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, string title, int status, string detail)
    {
        context.Response.StatusCode = status;
        await context.Response.WriteAsJsonAsync(new { title, status, detail });
    }
}