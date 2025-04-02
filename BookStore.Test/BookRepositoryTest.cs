using BookStore.Common;
using BookStore.Domain;
using BookStore.Persistence;
using Microsoft.Extensions.Options;
using Shouldly;

namespace BookStore.Test;

[TestFixture]
public class BookStoreRepositoryTests
{
    private BookStoreRepository _repository;
    private string _testRootFilePath;
    private string _testFilePath;
    private IOptions<BookStoreConfig> _config;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _testRootFilePath = @"D:\projects\BookStore\BookStore.Test\books.test.json";
        _testFilePath = _testRootFilePath.Replace(".json", ".tmp.json");
        _config = new OptionsWrapper<BookStoreConfig>(new BookStoreConfig { FilePath = _testFilePath });
        
        _repository = new BookStoreRepository(_config);
    }
    
    [SetUp]
    public void Setup()
    {
        File.Copy(_testRootFilePath, _testFilePath, true);
    }
    
    [TearDown]
    public void Cleanup()
    {
        if (File.Exists(_testFilePath))
        {
            File.Delete(_testFilePath);
        }
    }

    [Test]
    public async Task AddAsync_Should_Add_Book()
    {
        // Arrange
        var bookId = Guid.NewGuid();
        var book = new Book
        {
            Id = bookId,
            Title = "Test Book",
            Description = "Description",
            Author = "Author",
            Year = 2024
        };

        // Act
        await _repository.AddAsync(book);

        // Assert
        var retrievedBook = await _repository.GetByIdAsync(bookId);
        retrievedBook.ShouldNotBeNull();
        retrievedBook.Id.ShouldBe(book.Id);
        retrievedBook.Title.ShouldBe("Test Book");
    }

    [Test]
    public async Task UpdateAsync_Should_Update_Book()
    {
        // Arrange
        var bookId = Guid.NewGuid();
        var book = new Book
        {
            Id = bookId,
            Title = "Old Title",
            Description = "Old Description",
            Author = "Author",
            Year = 2024
        };
    
        await _repository.AddAsync(book);

        var updatedBook = new Book
        {
            Id = bookId,
            Title = "New Title",
            Description = "New Description",
            Author = "Author",
            Year = 2024
        };

        // Act
        await _repository.UpdateAsync(updatedBook);
        var fetchedBook = await _repository.GetByIdAsync(bookId);

        // Assert
        fetchedBook.ShouldNotBeNull();
        fetchedBook.Id.ShouldBe(bookId);
        fetchedBook.Title.ShouldBe("New Title");
        fetchedBook.Description.ShouldBe("New Description");
        fetchedBook.Author.ShouldBe("Author");
        fetchedBook.Year.ShouldBe(2024);
    }

    [Test]
    public async Task DeleteAsync_Should_Remove_Book()
    {
        // Arrange
        var book = new Book
        {
            Id = Guid.NewGuid(),
            Title = "Book to Delete",
            Description = "Description",
            Author = "Author",
            Year = 2024
        };
        await _repository.AddAsync(book);

        // Act
        await _repository.DeleteAsync(book.Id);
        var allBooks = await _repository.GetAllAsync();

        // Assert
        allBooks.ShouldNotContain(b => b.Id == book.Id);
    }

    [Test]
    public async Task GetByIdAsync_Should_Return_Correct_Book()
    {
        // Arrange
        var bookId = new Guid("11111111-1111-1111-1111-111111111111");

        // Act
        var fetchedBook = await _repository.GetByIdAsync(bookId);

        // Assert
        fetchedBook.ShouldNotBeNull();
        fetchedBook.Title.ShouldBe("Book 1");
        fetchedBook.Description.ShouldBe("Description 1");
        fetchedBook.Author.ShouldBe("Author 1");
        fetchedBook.Year.ShouldBe(2024);
    }

    [Test]
    public async Task GetAllAsync_Should_Return_All_Books()
    {
        // Arrange

        // Act
        var allBooks = await _repository.GetAllAsync();

        // Assert
        allBooks.Count.ShouldBe(3);
        allBooks[0].Title.ShouldBe("Book 1");
        allBooks[0].Description.ShouldBe("Description 1");
        allBooks[0].Author.ShouldBe("Author 1");
        allBooks[0].Year.ShouldBe(2024);
        
        allBooks[1].Title.ShouldBe("Book 2");
        allBooks[1].Description.ShouldBe("Description 2");
        allBooks[1].Author.ShouldBe("Author 2");
        allBooks[1].Year.ShouldBe(2024);
        
        allBooks[2].Title.ShouldBe("Book 3");
        allBooks[2].Description.ShouldBe("Description 3");
        allBooks[2].Author.ShouldBe("Author 3");
        allBooks[2].Year.ShouldBe(2025);
    }
}