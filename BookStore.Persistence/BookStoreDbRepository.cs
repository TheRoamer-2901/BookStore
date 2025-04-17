using BookStore.Contract;
using BookStore.Domain;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Persistence;

public class BookStoreDbRepository : IBookStoreDbRepository
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
        _context.Books.Update(book);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Book book, CancellationToken cancellationToken = default)
    {
        _context.Books.Remove(book);
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