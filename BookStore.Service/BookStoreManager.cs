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


    public async Task AddBookAsync(Book book, CancellationToken cancellationToken = default)
    {
        await _bookStoreRepository.AddAsync(book, cancellationToken);
    }

    public async Task UpdateBookAsync(Book book, CancellationToken cancellationToken = default)
    {
        await _bookStoreRepository.UpdateAsync(book, cancellationToken);
    }

    public async Task DeleteBookAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _bookStoreRepository.DeleteAsync(id, cancellationToken);
    }

    public async Task<Book> GetBookByIdAsync(Guid id, CancellationToken cancellationToken = default)
    { 
        return await _bookStoreRepository.GetByIdAsync(id, cancellationToken);
    }

    public async Task<IList<Book>> GetAllBooksAsync(CancellationToken cancellationToken = default)
    {
        return (await _bookStoreRepository.GetAllAsync(cancellationToken))
            .ToList();
    }
}