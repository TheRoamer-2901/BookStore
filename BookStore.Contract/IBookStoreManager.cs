using BookStore.Contract.DTOs;

namespace BookStore.Contract;

public interface IBookStoreManager
{
    void AddBook(BookDto book);
    void UpdateBook(BookDto book);
    void DeleteBook(Guid id);
    BookDto? GetBookById(Guid id);
    List<BookDto> GetAllBooks();
}