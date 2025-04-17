using BookStore.Domain;

namespace BookStore.Contract;

public interface IBookStoreRepository
{
    Task AddAsync(Book book, CancellationToken cancellationToken = default);
    Task UpdateAsync(Book bookToUpdate, CancellationToken cancellationToken = default);
    Task DeleteAsync(Book bookToDelete, CancellationToken cancellationToken = default);
    Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IList<Book>> GetAllAsync(CancellationToken cancellationToken = default);
}