using BookStore.Service;
using BookStore.Persistence;
using Microsoft.Extensions.DependencyInjection;

var serviceProvider = new ServiceCollection()
    .RegisterServiceLayer()
    .RegisterPersistenceLayer()
    .BuildServiceProvider();

var bookStoreManager = serviceProvider.GetService<IBookStoreManager>();

if (bookStoreManager is not null)
{
    bookStoreManager.GetAllBooks();
}
else
{
    Console.WriteLine("Cannot register the required service!!!");
}

