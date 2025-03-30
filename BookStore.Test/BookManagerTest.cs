using BookStore.Domain;
using BookStore.Persistence;
using BookStore.Service;
using NSubstitute;
using Serilog;
using Shouldly;

namespace BookStore.Test;

[TestFixture]
public class BookStoreManagerTests
{
    private IBookStoreRepository _mockRepository;
    private ILogger _mockLogger;
    private BookStoreManager _bookStoreManager;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _mockRepository = Substitute.For<IBookStoreRepository>();
        _mockLogger = Substitute.For<ILogger>();
        _bookStoreManager = new BookStoreManager(_mockRepository, _mockLogger);
    }

    [Test]
    public async Task AddBookAsync_ShouldCallRepositoryAddAsync()
    {
        // Arrange
        var book = new Book { Id = Guid.NewGuid(), Title = "Test Book" };
            
        // Act
        await _bookStoreManager.AddBookAsync(book);
            
        // Assert
        await _mockRepository.Received(1).AddAsync(book, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task UpdateBookAsync_ShouldCallRepositoryUpdateAsync()
    {
        // Arrange
        var book = new Book { Id = Guid.NewGuid(), Title = "Updated Book" };
            
        // Act
        await _bookStoreManager.UpdateBookAsync(book);
            
        // Assert
        await _mockRepository.Received(1).UpdateAsync(book, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task DeleteBookAsync_ShouldCallRepositoryDeleteAsync()
    {
        // Arrange
        var bookId = Guid.NewGuid();
            
        // Act
        await _bookStoreManager.DeleteBookAsync(bookId);
            
        // Assert
        await _mockRepository.Received(1).DeleteAsync(bookId, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task GetBookByIdAsync_ShouldReturnBookFromRepository()
    {
        // Arrange
        var bookId = Guid.NewGuid();
        var book = new Book { Id = bookId, Title = "Test Book" };
        _mockRepository.GetByIdAsync(bookId, Arg.Any<CancellationToken>()).Returns(book);

        // Act
        var result = await _bookStoreManager.GetBookByIdAsync(bookId);
            
        // Assert
        await _mockRepository.Received(1).GetByIdAsync(bookId, Arg.Any<CancellationToken>());
        result.Id.ShouldBe(book.Id);
        result.Title.ShouldBe(book.Title);
    }

    [Test]
    public async Task GetAllBooksAsync_ShouldReturnAllBooksFromRepository()
    {
        // Arrange
        var books = new List<Book>
        {
            new() { Id = Guid.NewGuid(), Title = "Book 1" },
            new() { Id = Guid.NewGuid(), Title = "Book 2" }
        };
        _mockRepository.GetAllAsync(Arg.Any<CancellationToken>()).Returns(books);

        // Act
        var result = await _bookStoreManager.GetAllBooksAsync();
            
        // Assert
        await _mockRepository.Received(1).GetAllAsync(Arg.Any<CancellationToken>());
        result.Count.ShouldBe(books.Count);
    }
}