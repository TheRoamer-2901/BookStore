using BookStore.Contract;
using BookStore.Domain;
using Microsoft.Extensions.Caching.Memory;

namespace BookStore.Persistence;

public class CachedBookStoreRepository : IBookStoreRepository
{
    private readonly IBookStoreRepository _innerRepo;
    private readonly IMemoryCache _cache;

    public CachedBookStoreRepository(IBookStoreRepository innerRepo, IMemoryCache cache)
    {
        _innerRepo = innerRepo;
        _cache = cache;
    }

    public async Task AddAsync(Book book, CancellationToken cancellationToken = default)
    {
        await _innerRepo.AddAsync(book, cancellationToken);
        UpdateCachedEntry(book.Id, book);
    }

    public async Task UpdateAsync(Book bookToUpdate, CancellationToken cancellationToken = default)
    {
        await _innerRepo.UpdateAsync(bookToUpdate, cancellationToken);
        UpdateCachedEntry(bookToUpdate.Id, bookToUpdate);
    }

    public async Task DeleteAsync(Book bookToDelete, CancellationToken cancellationToken = default)
    {
        await _innerRepo.DeleteAsync(bookToDelete, cancellationToken);
        UpdateCachedEntry(bookToDelete.Id);
    }

    public async Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _cache.GetOrCreateAsync(id, async entry =>
        {
            entry.Size = 1;
            entry.SlidingExpiration = TimeSpan.FromMinutes(1);
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
            return await _innerRepo.GetByIdAsync(id, cancellationToken);
        });
    }

    public async Task<IList<Book>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _innerRepo.GetAllAsync(cancellationToken);
    }

    private void UpdateCachedEntry(Guid cacheKey, Book? cachePayload = null)
    {
        _cache.Remove(cacheKey);

        if (cachePayload is null)
        {
            return;
        }
        
        _cache.Set(cacheKey, cachePayload, new MemoryCacheEntryOptions
        {
            Size = 1,
            SlidingExpiration = TimeSpan.FromMinutes(1),
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
        });
    }
}
