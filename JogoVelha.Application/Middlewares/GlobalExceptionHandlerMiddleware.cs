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
        catch (ValidationException ex) 
        {
            Console.WriteLine("AQUII");
        }
        catch (ArgumentException ex) 
        {
            await HandleExceptionAsync(context, "Argument invalid", 400, ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            await HandleExceptionAsync(context, "Server error", 500, "A server error occurred.");
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, string title, int status, string detail)
    {
        context.Response.StatusCode = status;
        await context.Response.WriteAsJsonAsync(new { title, status, detail });
    }
}