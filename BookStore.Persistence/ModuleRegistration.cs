using Microsoft.Extensions.DependencyInjection;

namespace BookStore.Persistence;

public static class ModuleRegistration
{
    public static IServiceCollection RegisterPersistenceLayer(this IServiceCollection services)
    {
        services.AddScoped<IBookStoreRepository, BookStoreRepository>();
        return services;
    }
}