using BookStore.Domain;

namespace BookStore.Contract;

public interface IBookStoreRepository
{
    Task AddAsync(IList<Book> books, CancellationToken cancellationToken = default);
    Task UpdateAsync(IList<Book> updatedBooks, CancellationToken cancellationToken = default);
    Task DeleteAsync(IList<Book> books, CancellationToken cancellationToken = default);
    Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IList<Book>> GetAllAsync(CancellationToken cancellationToken = default);
}