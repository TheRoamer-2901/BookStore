using Microsoft.Extensions.DependencyInjection;

namespace BookStore.Service;

public static class ModuleRegistration
{
    public static IServiceCollection RegisterServiceLayer(this IServiceCollection services)
    {
        services.AddScoped<IBookStoreManager, BookStoreManager>();
        return services;
    }
}