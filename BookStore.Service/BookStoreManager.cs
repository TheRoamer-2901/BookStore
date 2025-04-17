using BookStore.Common.Exceptions;
using BookStore.Contract;
using BookStore.Domain;

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
        var book = await _bookStoreRepository.GetByIdAsync(newBook.Id, cancellationToken);
        if (book is not null)
        {
            throw new BookDuplicatedException(newBook.Id);
        }
        await _bookStoreRepository.AddAsync(newBook, cancellationToken);
    }

    public async Task UpdateBookAsync(Book bookToUpdate, CancellationToken cancellationToken = default)
    {
        var book = await _bookStoreRepository.GetByIdAsync(bookToUpdate.Id, cancellationToken);
        if (book is null)
        {
            throw new BookNotFoundException(bookToUpdate.Id);
        }
        await _bookStoreRepository.UpdateAsync(bookToUpdate, cancellationToken);
    }

    public async Task DeleteBookAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var book = await _bookStoreRepository.GetByIdAsync(id, cancellationToken);
        if (book is null)
        {
            throw new BookNotFoundException(id);
        }
        await _bookStoreRepository.DeleteAsync(book, cancellationToken);
    }

    public async Task<Book> GetBookByIdAsync(Guid id, CancellationToken cancellationToken = default)
    { 
        return await _bookStoreRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new BookNotFoundException(id);
    }

    public async Task<IList<Book>> GetAllBooksAsync(CancellationToken cancellationToken = default)
    {
        return await _bookStoreRepository.GetAllAsync(cancellationToken);
    }
}