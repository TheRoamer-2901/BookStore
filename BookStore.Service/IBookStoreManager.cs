using BookStore.Domain;

namespace BookStore.Service;

public interface IBookStoreManager
{
    void AddBook(Book book);
    void UpdateBook(Book book);
    void DeleteBook(Guid id);
    Book? GetBookById(Guid id);
    List<Book> GetAllBooks();
}