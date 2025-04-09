namespace BookStore.Api.Middlewares;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseBookStoreMiddlewares(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseMiddleware<TimeElapsedLoggingMiddleware>();

        return app;
    }
}