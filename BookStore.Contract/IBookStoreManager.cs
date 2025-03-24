using BookStore.Contract.DTOs;
using BookStore.Domain;

namespace BookStore.Contract;

public interface IBookStoreManager
{
    Task AddBookAsync(Book book, CancellationToken cancellationToken = default);
    Task UpdateBookAsync(Book book, CancellationToken cancellationToken = default);
    Task DeleteBookAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Book> GetBookByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IList<Book>> GetAllBooksAsync(CancellationToken cancellationToken = default);
}