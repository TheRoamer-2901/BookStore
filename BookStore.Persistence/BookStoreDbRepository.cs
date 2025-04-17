using BookStore.Contract;
using BookStore.Domain;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Persistence;

public class BookStoreDbRepository : IBookStoreRepository
{
    private readonly BookStoreDbContext _context;

    public BookStoreDbRepository(BookStoreDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Book book, CancellationToken cancellationToken = default)
    {
        _context.Books.Add(book);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Book book, CancellationToken cancellationToken = default)
    {
        var existing = await _context.Books.FirstAsync(x => x.Id == book.Id, cancellationToken);
        _context.Entry(existing).CurrentValues.SetValues(book);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Book bookToDelete, CancellationToken cancellationToken = default)
    {
        _context.Books.Remove(bookToDelete);
        await _context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Books.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    public async Task<IList<Book>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Books.ToListAsync(cancellationToken);
    }
}