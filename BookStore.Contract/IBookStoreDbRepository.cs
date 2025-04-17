using BookStore.Domain;

namespace BookStore.Contract;

public interface IBookStoreDbRepository
{
    Task AddAsync(Book book, CancellationToken cancellationToken = default);
    Task UpdateAsync(Book book, CancellationToken cancellationToken = default);
    Task DeleteAsync(Book book, CancellationToken cancellationToken = default);
    Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IList<Book>> GetAllAsync(CancellationToken cancellationToken = default);
}