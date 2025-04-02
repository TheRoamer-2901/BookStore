using BookStore.Common.Exceptions;
using BookStore.Contract;
using BookStore.Domain;
using BookStore.Service;
using NSubstitute;
using Shouldly;

namespace BookStore.Test;

[TestFixture]
public class BookStoreManagerTests
{
    private IBookStoreRepository _bookStoreRepository;
    private BookStoreManager _bookStoreManager;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _bookStoreRepository = Substitute.For<IBookStoreRepository>();
        _bookStoreManager = new BookStoreManager(_bookStoreRepository);
    }

    [TearDown]
    public void TearDown()
    {
        _bookStoreRepository.ClearReceivedCalls();
    }
    
    [Test]
    public async Task AddBookAsync_WhenBookExists_ShouldThrowException()
    {
        // Arrange
        var book = new Book { Id = Guid.NewGuid() };
        _bookStoreRepository.GetAllAsync(Arg.Any<CancellationToken>()).Returns(new List<Book> { book });

        // Act
        var addBookAction = async () => await _bookStoreManager.AddBookAsync(book);
        
        // Assert
        await Should.ThrowAsync<BookDuplicatedException>(addBookAction);
    }

    [Test]
    public async Task AddBookAsync_WhenBookDoesNotExist_ShouldAddBook()
    {
        // Arrange
        var book = new Book { Id = Guid.NewGuid() };
        _bookStoreRepository.GetAllAsync(Arg.Any<CancellationToken>()).Returns(new List<Book>());

        // Act
        await _bookStoreManager.AddBookAsync(book);

        // Assert
        await _bookStoreRepository.Received(1).AddAsync(Arg.Is<List<Book>>(b => b.Contains(book)), Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task UpdateBookAsync_WhenBookDoesNotExist_ShouldThrowException()
    {
        // Arrange
        var book = new Book { Id = Guid.NewGuid() };
        _bookStoreRepository.GetAllAsync(Arg.Any<CancellationToken>()).Returns(new List<Book>());

        // Act
        var updateBookAction = async () => await _bookStoreManager.UpdateBookAsync(book);
        
        // Assert
        await Should.ThrowAsync<BookNotFoundException>(updateBookAction);
    }

    [Test]
    public async Task UpdateBookAsync_WhenBookExists_ShouldUpdateBook()
    {
        // Arrange
        var book = new Book { Id = Guid.NewGuid() };
        var existingBooks = new List<Book> { book };
        _bookStoreRepository.GetAllAsync(Arg.Any<CancellationToken>()).Returns(existingBooks);

        // Act
        await _bookStoreManager.UpdateBookAsync(book);

        // Assert
        await _bookStoreRepository.Received(1).UpdateAsync(Arg.Is<List<Book>>(b => b.Contains(book)), Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task DeleteBookAsync_WhenBookDoesNotExist_ShouldThrowException()
    {
        // Arrange
        var bookId = Guid.NewGuid();
        _bookStoreRepository.GetAllAsync(Arg.Any<CancellationToken>()).Returns(new List<Book>());

        // Act
        var deleteBookAction = async () => await _bookStoreManager.DeleteBookAsync(bookId);
        
        // Assert
        await Should.ThrowAsync<BookNotFoundException>(deleteBookAction);
    }

    [Test]
    public async Task DeleteBookAsync_WhenBookExists_ShouldDeleteBook()
    {
        // Arrange
        var book = new Book { Id = Guid.NewGuid() };
        var existingBooks = new List<Book> { book };
        _bookStoreRepository.GetAllAsync(Arg.Any<CancellationToken>()).Returns(existingBooks);

        // Act
        await _bookStoreManager.DeleteBookAsync(book.Id);

        // Assert
        await _bookStoreRepository.Received(1).DeleteAsync(Arg.Is<List<Book>>(b => b.All(x => x != book)), Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task GetBookByIdAsync_WhenBookDoesNotExist_ShouldThrowException()
    {
        // Arrange
        var bookId = Guid.NewGuid();
        _bookStoreRepository.GetByIdAsync(bookId, Arg.Any<CancellationToken>()).Returns(default(Book));

        // Act
        Func<Task<Book>> getBookByIdAtion = async () => await _bookStoreManager.GetBookByIdAsync(bookId);
        
        // Assert
        await Should.ThrowAsync<BookNotFoundException>(getBookByIdAtion);
    }

    [Test]
    public async Task GetBookByIdAsync_WhenBookExists_ShouldReturnBook()
    {
        // Arrange
        var book = new Book { Id = Guid.NewGuid() };
        _bookStoreRepository.GetByIdAsync(book.Id, Arg.Any<CancellationToken>()).Returns(book);

        // Act
        var result = await _bookStoreManager.GetBookByIdAsync(book.Id);

        // Assert
        result.ShouldBe(book);
    }

    [Test]
    public async Task GetAllBooksAsync_WhenCalled_ShouldReturnAllBooks()
    {
        // Arrange
        var books = new List<Book>
        {
            new() { Id = Guid.NewGuid(), Title = "Book 1" },
            new() { Id = Guid.NewGuid(), Title = "Book 2" }
        };
        _bookStoreRepository.GetAllAsync(Arg.Any<CancellationToken>()).Returns(books);

        // Act
        var result = await _bookStoreManager.GetAllBooksAsync();
            
        // Assert
        await _bookStoreRepository.Received(1).GetAllAsync(Arg.Any<CancellationToken>());
        result.Count.ShouldBe(books.Count);
    }
}