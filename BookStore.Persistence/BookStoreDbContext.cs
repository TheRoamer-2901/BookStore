using BookStore.Domain;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Persistence;

public class BookStoreDbContext : DbContext
{
    public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options)
        : base(options)
    {
    }

    public DbSet<Book> Books { get; set; }
}