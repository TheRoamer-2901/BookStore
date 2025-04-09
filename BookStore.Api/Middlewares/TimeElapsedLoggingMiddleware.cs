using System.Diagnostics;
using ILogger = Serilog.ILogger;

namespace BookStore.Api.Middlewares;

public class TimeElapsedLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public TimeElapsedLoggingMiddleware(RequestDelegate next, ILogger logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        await _next(context);

        stopwatch.Stop();

        var elapsedMs = stopwatch.ElapsedMilliseconds;
        var requestPath = context.Request.Path;
        var statusCode = context.Response.StatusCode;

        _logger.Information($"Request to {requestPath} responded {statusCode} in {elapsedMs:0.000} ms",
            requestPath, statusCode, elapsedMs);
    }
}