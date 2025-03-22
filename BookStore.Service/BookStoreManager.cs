using BookStore.Domain;

namespace BookStore.Service;

public class BookStoreManager : IBookStoreManager
{
    public void AddBook(Book book)
    {
        throw new NotImplementedException();
    }

    public void UpdateBook(Book book)
    {
        throw new NotImplementedException();
    }

    public void DeleteBook(Guid id)
    {
        throw new NotImplementedException();
    }

    public Book? GetBookById(Guid id)
    {
        throw new NotImplementedException();
    }

    public List<Book> GetAllBooks()
    {
        Console.WriteLine("Retrieving all books...");
        return [];
    }
}