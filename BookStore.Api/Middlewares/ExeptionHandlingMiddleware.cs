using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Net;
using System.Text.Json;
using BookStore.Common.Exceptions;

namespace BookStore.Api.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (BookDuplicatedException ex)
        {
            Log.Warning(ex, "Book duplication error: {BookId}", ex.Id);
            await HandleExceptionAsync(context, ex, HttpStatusCode.Conflict, "Book already exists");
        }
        catch (BookNotFoundException ex)
        {
            Log.Warning(ex, "Book not found: {BookId}", ex.Id);
            await HandleExceptionAsync(context, ex, HttpStatusCode.NotFound, "Book not found");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Unhandled exception occurred while processing request to {Path}", context.Request.Path);
            await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError, "An unexpected error occurred");
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex, HttpStatusCode statusCode, string title)
    {
        var problemDetails = new ProblemDetails
        {
            Status = (int)statusCode,
            Title = title,
            Detail = ex.Message,
            Instance = context.Request.Path
        };

        await WriteProblemDetailsAsync(context, problemDetails);
    }

    private async Task WriteProblemDetailsAsync(HttpContext context, ProblemDetails problemDetails)
    {
        context.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/problem+json";

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var json = JsonSerializer.Serialize(problemDetails, options);
        await context.Response.WriteAsync(json);
    }
}
