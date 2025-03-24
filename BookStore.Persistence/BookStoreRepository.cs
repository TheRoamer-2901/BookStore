using System.Text.Json;
using BookStore.Domain;

namespace BookStore.Persistence;

public class BookStoreRepository : IBookStoreRepository
{
    private const string FilePath = @"D:\projects\BookStore\BookStore.Persistence\books.json";
    private string TempFilePath => FilePath.Replace(".json", ".tmp.json");
    
    public async Task AddAsync(Book book, CancellationToken cancellationToken = default)
    {
        var books = (await LoadBooksAsync()).ToList();
        books.Add(book);
        await WriteBooksToFileAsync(books, FilePath, cancellationToken);
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
        var books = await GetAllAsync();
        var book = books.FirstOrDefault(b => b.Id == updatedBook.Id);
    
        if (book is null)
        {
            throw new InvalidOperationException("Book not found.");
        }

        var updatedBooks = books.Select(b => b.Id != updatedBook.Id ? b : updatedBook).ToList();

        await WriteBooksToFileAsync(updatedBooks, TempFilePath, cancellationToken);

        File.Delete(FilePath);
        File.Move(TempFilePath, FilePath);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var books = await GetAllAsync();
        var bookToDelete = books.FirstOrDefault(b => b.Id == id);

        if (bookToDelete is null)
        {
            throw new InvalidOperationException("Book not found.");
        }

        var updatedBooks = books.Where(b => b.Id != id).ToList();

        await WriteBooksToFileAsync(updatedBooks, TempFilePath, cancellationToken);
        OverwriteRootFile(FilePath, TempFilePath);
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
        if (!File.Exists(FilePath))
        {
            return [];
        }

        var books = new List<Book>();

        using var reader = new StreamReader(FilePath);
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
