using AutoMapper;
using BookStore.Common;
using BookStore.Contract;
using BookStore.Contract.DTOs;
using BookStore.Domain;
using BookStore.Service;
using BookStore.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

var serviceProvider = new ServiceCollection()
    .RegisterCommonServices()
    .RegisterContractLayer()
    .RegisterServiceLayer()
    .RegisterPersistenceLayer()
    .BuildServiceProvider();

var mapper = serviceProvider.GetRequiredService<IMapper>();

var bookStoreManager = serviceProvider.GetService<IBookStoreManager>();

if (bookStoreManager is not null)
{
    Log.Information("App started successfully");
    
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
    Log.Error("Cannot register the required service!!!");
}

Log.CloseAndFlush();

