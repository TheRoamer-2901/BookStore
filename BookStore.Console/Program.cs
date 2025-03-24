using AutoMapper;
using BookStore.Contract;
using BookStore.Contract.DTOs;
using BookStore.Domain;
using BookStore.Service;
using BookStore.Persistence;
using Microsoft.Extensions.DependencyInjection;

var serviceProvider = new ServiceCollection()
    .RegisterContractLayer()
    .RegisterServiceLayer()
    .RegisterPersistenceLayer()
    .BuildServiceProvider();

var mapper = serviceProvider.GetRequiredService<IMapper>();

var bookStoreManager = serviceProvider.GetService<IBookStoreManager>();

if (bookStoreManager is not null)
{
    var newBookDto = new BookDto(
        Guid.NewGuid(),
        "The Pragmatic Programmer",
        "A classic book on software development.",
        "Andrew Hunt & David Thomas",
        1999);

    await bookStoreManager.AddBookAsync(mapper.Map<Book>(newBookDto));
    
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

