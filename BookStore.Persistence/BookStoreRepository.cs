using System.Text.Json;
using BookStore.Common;
using BookStore.Contract;
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
    
    public async Task AddAsync(IList<Book> books, CancellationToken cancellationToken = default)
    {
        await WriteBooksToFileAsync(books, TempFilePath, cancellationToken);
        OverwriteRootFile(_filePath, TempFilePath);
    }

    private async Task WriteBooksToFileAsync(IList<Book> books, string filePath,
        CancellationToken cancellationToken = default)
    {
        await using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
        await JsonSerializer.SerializeAsync(fileStream, books, new JsonSerializerOptions
        {
            WriteIndented = true
        }, cancellationToken);
    }

    public async Task UpdateAsync(IList<Book> updatedBooks, CancellationToken cancellationToken = default)
    {
        await WriteBooksToFileAsync(updatedBooks, TempFilePath, cancellationToken);
        OverwriteRootFile(_filePath, TempFilePath);
    }

    public async Task DeleteAsync(IList<Book> books, CancellationToken cancellationToken = default)
    {
        await WriteBooksToFileAsync(books, TempFilePath, cancellationToken);
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

        await using var fileStream = new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        var books = await JsonSerializer.DeserializeAsync<List<Book>>(fileStream, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }, cancellationToken);

        return books ?? [];
    }
    
    private void OverwriteRootFile(string rootFilePath, string tempFilePath)
    {
        File.Delete(rootFilePath);
        File.Move(tempFilePath, rootFilePath);
    }
}
