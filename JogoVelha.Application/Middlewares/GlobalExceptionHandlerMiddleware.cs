using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.AspNetCore.Mvc;

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
            
        }
        catch (ArgumentException ex) 
        {
            await HandleOnException(context, "Argument invalid", 400, ex.Message);
        }
        catch
        {
            await HandleOnException(context, "Server error", 500, "A server error occurred.");
        }
    }

    private async Task HandleOnException(HttpContext context, string title, int status, string detail)
    {
        context.Response.StatusCode = status;

         ProblemDetails problemDetails = new() 
            {
                Title = title,
                Type = "error",
                Status = status,
                Detail = detail
            };

        await context.Response.WriteAsJsonAsync(problemDetails);
    }
}