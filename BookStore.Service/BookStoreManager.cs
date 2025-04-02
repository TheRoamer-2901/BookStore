using BookStore.Contract;
using BookStore.Contract.DTOs;
using BookStore.Domain;
using BookStore.Persistence;
using Serilog;

namespace BookStore.Service;

public class BookStoreManager : IBookStoreManager
{
    private readonly IBookStoreRepository _bookStoreRepository;

    public BookStoreManager(IBookStoreRepository bookStoreRepository)
    {
        _bookStoreRepository = bookStoreRepository;
    }

    public async Task AddBookAsync(Book newBook, CancellationToken cancellationToken = default)
    {
        var books = await _bookStoreRepository.GetAllAsync(cancellationToken);
        var book = books.FirstOrDefault(x => x.Id == newBook.Id);
        if (book is not null)
        {
            throw new Exception("Cannot insert duplicated book");
        }
        books.Add(newBook);
        await _bookStoreRepository.AddAsync(books, cancellationToken);
    }

    public async Task UpdateBookAsync(Book bookToUpdate, CancellationToken cancellationToken = default)
    {
        var books = await _bookStoreRepository.GetAllAsync(cancellationToken);
        var book = books.FirstOrDefault(x => x.Id == bookToUpdate.Id);
        if (book is null)
        {
            throw new Exception("Book not found");
        }
        books = books.Select(b => b.Id == bookToUpdate.Id ? bookToUpdate : b).ToList();
        await _bookStoreRepository.UpdateAsync(books, cancellationToken);
    }

    public async Task DeleteBookAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var books = await _bookStoreRepository.GetAllAsync(cancellationToken);
        var book = books.FirstOrDefault(x => x.Id == id);
        if (book is null)
        {
            throw new Exception("Book not found");
        }
        books = books.Where(x => x.Id != id).ToList();
        await _bookStoreRepository.DeleteAsync(books, cancellationToken);
    }

    public async Task<Book> GetBookByIdAsync(Guid id, CancellationToken cancellationToken = default)
    { 
        return await _bookStoreRepository.GetByIdAsync(id, cancellationToken);
    }

    public async Task<IList<Book>> GetAllBooksAsync(CancellationToken cancellationToken = default)
    {
        return await _bookStoreRepository.GetAllAsync(cancellationToken);
    }
}