using BookStore.Common;
using BookStore.Contract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookStore.Persistence;

public static class ModuleRegistration
{
    public static IServiceCollection RegisterPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<BookStoreConfig>(configuration.GetSection("BookStoreSettings"));
        services.AddScoped<IBookStoreRepository, BookStoreRepository>();
        return services;
    }
}