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
    private IList<Book> _books;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _testRootFilePath = @"D:\projects\BookStore\BookStore.Test\books.test.json";
        _testFilePath = _testRootFilePath.Replace(".json", ".tmp.json");
        _config = new OptionsWrapper<BookStoreConfig>(new BookStoreConfig { FilePath = _testFilePath });
        _books =
        [
            new Book
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Title = "Book 1",
                Description = "Description 1",
                Author = "Author 1",
                Year = 2024
            },
            new Book
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Title = "Book 2",
                Description = "Description 2",
                Author = "Author 2",
                Year = 2024
            },
            new Book
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Title = "Book 3",
                Description = "Description 3",
                Author = "Author 3",
                Year = 2025
            }
        ];
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
    public async Task AddAsync_WhenCalled_ShouldAddBook()
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
        var retrievedBooks = await _repository.GetAllAsync();
        var addedBook = retrievedBooks.FirstOrDefault(x => x.Id == bookId);
        retrievedBooks.Count.ShouldBe(4);
        addedBook.ShouldNotBeNull();
        addedBook.Id.ShouldBe(book.Id);
        addedBook.Title.ShouldBe("Test Book");
    }
    
    [Test]
    public async Task UpdateAsync_WhenCalled_ShouldUpdateBook()
    {
        // Arrange
        var bookToUpdate = _books.First();
        bookToUpdate.Title = "New Title";
        bookToUpdate.Description = "New Description";
        bookToUpdate.Author = "Author";
        bookToUpdate.Year = 2024;

        // Act
        await _repository.UpdateAsync(bookToUpdate);


        // Assert
        var updatedBook = await _repository.GetByIdAsync(bookToUpdate.Id);
        updatedBook.ShouldNotBeNull();
        updatedBook.Title.ShouldBe("New Title");
        updatedBook.Description.ShouldBe("New Description");
        updatedBook.Author.ShouldBe("Author");
        updatedBook.Year.ShouldBe(2024);
    }
    
    [Test]
    public async Task DeleteAsync_Should_Remove_Book()
    {
        // Arrange
        var bookToDeleteId = new Guid("11111111-1111-1111-1111-111111111111");
        var bookToDelete = _books.First(x => x.Id == bookToDeleteId);
    
        // Act
        await _repository.DeleteAsync(bookToDelete);
    
        // Assert
        var newBooks = await _repository.GetAllAsync();
        newBooks.Count.ShouldBe(2);
        newBooks.ShouldNotContain(b => b.Id == bookToDeleteId);
    }


    [Test]
    public async Task GetByIdAsync_WhenBookExist_ShouldReturnCorrectBook()
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
    public async Task GetAllAsync_WhenCalled_ShouldReturnAllBooks()
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