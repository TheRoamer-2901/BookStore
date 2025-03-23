using BookStore.Domain;
using BookStore.Persistence;

namespace BookStore.Service;

public class BookStoreManager : IBookStoreManager
{
    private readonly IBookStoreRepository _bookStoreRepository;

    public BookStoreManager(IBookStoreRepository bookStoreRepository)
    {
        _bookStoreRepository = bookStoreRepository;
    }

    public void AddBook(Book book)
    {
        throw new NotImplementedException();
    }

    public void UpdateBook(Book book)
    {
        _bookStoreRepository.Update(book);
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
        return _bookStoreRepository.GetAll();
    }
}