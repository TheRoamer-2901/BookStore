using BookStore.Contract;
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
    foreach (var book in await bookStoreManager.GetAllBooksAsync())
    {
        Console.WriteLine($@"
        Id: {book.Id}
        Title: {book.Title}
        Description: {book.Description}
        Author: {book.Author}
        Year: {book.Year}
        ");
    }
}
else
{
    Console.WriteLine("Cannot register the required service!!!");
}

