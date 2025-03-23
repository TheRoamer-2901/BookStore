using BookStore.Contract;
using BookStore.Contract.DTOs;
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

    public void AddBook(BookDto book)
    {
        _bookStoreRepository.Add(ToBookDomain(book));
    }

    public void UpdateBook(BookDto book)
    {
        _bookStoreRepository.Update(ToBookDomain(book));
    }

    public void DeleteBook(Guid id)
    {
        _bookStoreRepository.Delete(id);
    }

    public BookDto? GetBookById(Guid id)
    {
        var book = _bookStoreRepository.GetById(id);
        return book is not null ? ToBookDto(book) : null;
    }

    public List<BookDto> GetAllBooks()
    {
        return _bookStoreRepository.GetAll()
            .Select(ToBookDto)
            .ToList();
    }

    private Book ToBookDomain(BookDto bookDto)
    {
        return new Book
        {
            Id = bookDto.Id,
            Title = bookDto.Title,
            Author = bookDto.Author,
            Description = bookDto.Description,
            Year = bookDto.Year
        };
    }

    private BookDto ToBookDto(Book book)
    {
        return new BookDto(
            book.Id,
            book.Title,
            book.Author,
            book.Description,
            book.Year
        );
    }
}