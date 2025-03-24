using BookStore.Domain;

namespace BookStore.Persistence;

public interface IBookStoreRepository
{
    Task AddAsync(Book book, CancellationToken cancellationToken = default);
    Task UpdateAsync(Book updatedBook, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IList<Book>> GetAllAsync(CancellationToken cancellationToken = default);
}