using System.Text.Json;
using BookStore.Common;
using BookStore.Domain;
using Microsoft.Extensions.Options;

namespace BookStore.Persistence;

public class BookStoreRepository : IBookStoreRepository
{
    private readonly string _filePath;
    private string TempFilePath => _filePath.Replace(".json", ".tmp.json");

    public BookStoreRepository(IOptions<BookStoreConfig> config)
    {
        _filePath = config.Value.FilePath;
    }
    
    public async Task AddAsync(Book book, CancellationToken cancellationToken = default)
    {
        var books = (await LoadBooksAsync(cancellationToken)).ToList();
        books.Add(book);
        await WriteBooksToFileAsync(books, _filePath, cancellationToken);
    }

    private async Task WriteBooksToFileAsync(List<Book> books, string filePath,
        CancellationToken cancellationToken = default)
    {
        await using var writer = new StreamWriter(filePath, false);
        
        await writer.WriteLineAsync("[");

        for (int i = 0; i < books.Count; i++)
        {
            string serializedBook = $"  {JsonSerializer.Serialize(books[i])}";

            if (i < books.Count - 1)
            {
                serializedBook += ",";
            }

            await writer.WriteLineAsync(serializedBook);
        }

        await writer.WriteLineAsync("]");
    }

    public async Task UpdateAsync(Book updatedBook, CancellationToken cancellationToken = default)
    {
        var books = await GetAllAsync(cancellationToken);
        var book = books.FirstOrDefault(b => b.Id == updatedBook.Id);
    
        if (book is null)
        {
            throw new InvalidOperationException("Book not found.");
        }

        var updatedBooks = books.Select(b => b.Id != updatedBook.Id ? b : updatedBook).ToList();

        await WriteBooksToFileAsync(updatedBooks, TempFilePath, cancellationToken);

        File.Delete(_filePath);
        File.Move(TempFilePath, _filePath);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var books = await GetAllAsync(cancellationToken);
        var bookToDelete = books.FirstOrDefault(b => b.Id == id);

        if (bookToDelete is null)
        {
            throw new InvalidOperationException("Book not found.");
        }

        var updatedBooks = books.Where(b => b.Id != id).ToList();

        await WriteBooksToFileAsync(updatedBooks, TempFilePath, cancellationToken);
        OverwriteRootFile(_filePath, TempFilePath);
    }

    public async Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var books = await LoadBooksAsync(cancellationToken);
        return books.FirstOrDefault(x => x.Id == id);
    }

    public async Task<IList<Book>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return (await LoadBooksAsync(cancellationToken)).ToList();
    }

    private async Task<IEnumerable<Book>> LoadBooksAsync(CancellationToken cancellationToken = default)
    {
        if (!File.Exists(_filePath))
        {
            return [];
        }

        var books = new List<Book>();

        using var reader = new StreamReader(_filePath);
        string? line;

        while ((line = await reader.ReadLineAsync(cancellationToken)) != null)
        {
            line = line.Trim();
            if (line == "[" || line == "]") continue;

            if (line.EndsWith(","))
            {
                line = line[..^1];
            }
            var book = JsonSerializer.Deserialize<Book>(line);
            if (book != null)
            {
                books.Add(book);
            }
        }
        
        return books;
    }
    
    private void OverwriteRootFile(string rootFilePath, string tempFilePath)
    {
        File.Delete(rootFilePath);
        File.Move(tempFilePath, rootFilePath);
    }
}
