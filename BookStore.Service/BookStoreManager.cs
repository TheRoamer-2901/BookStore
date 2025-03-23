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
        _bookStoreRepository.Add(book);
    }

    public void UpdateBook(Book book)
    {
        _bookStoreRepository.Update(book);
    }

    public void DeleteBook(Guid id)
    {
        _bookStoreRepository.Delete(id);
    }

    public Book? GetBookById(Guid id)
    {
        return _bookStoreRepository.GetById(id);
    }

    public List<Book> GetAllBooks()
    {
        return _bookStoreRepository.GetAll();
    }
}