using System.Text.Json;
using BookStore.Domain;

namespace BookStore.Persistence;

public class BookStoreRepository : IBookStoreRepository
{
    private const string FilePath = @"D:\projects\BookStore\BookStore.Persistence\books.json";
    private string TempFilePath => FilePath.Replace(".json", ".tmp.json");

    public BookStoreRepository()
    {
    }

    public void Add(Book book)
    {
        var books = LoadBooks().ToList();
        books.Add(book);
        WriteBooksToFile(books, FilePath);
    }

    private static void WriteBooksToFile(List<Book> books, string filePath)
    {
        using var writer = new StreamWriter(filePath, false);
        
        writer.WriteLine("[");

        for (int i = 0; i < books.Count; i++)
        {
            string serializedBook = $"  {JsonSerializer.Serialize(books[i])}";

            if (i < books.Count - 1)
            {
                serializedBook += ",";
            }

            writer.WriteLine(serializedBook);
        }

        writer.WriteLine("]");
    }

    public void Update(Book updatedBook)
    {
        var books = GetAll();
        var book = books.FirstOrDefault(b => b.Id == updatedBook.Id);
    
        if (book is null)
        {
            throw new InvalidOperationException("Book not found.");
        }

        var updatedBooks = books.Select(b => b.Id != updatedBook.Id ? b : updatedBook).ToList();

        WriteBooksToFile(updatedBooks, TempFilePath);

        File.Delete(FilePath);
        File.Move(TempFilePath, FilePath);
    }

    public void Delete(Guid id)
    {
        var books = GetAll();
        var bookToDelete = books.FirstOrDefault(b => b.Id == id);

        if (bookToDelete is null)
        {
            throw new InvalidOperationException("Book not found.");
        }

        var updatedBooks = books.Where(b => b.Id != id).ToList();

        WriteBooksToFile(updatedBooks, TempFilePath);

        OverwriteRootFile(FilePath, TempFilePath);
    }

    public Book? GetById(Guid id)
    {
        var books = LoadBooks();
        return books.FirstOrDefault(x => x.Id == id);
    }

    public List<Book> GetAll()
    {
        return LoadBooks().ToList();
    }

    private IEnumerable<Book> LoadBooks()
    {
        if (!File.Exists(FilePath))
        {
            yield break;
        }

        using var reader = new StreamReader(FilePath);
        string? line;

        while ((line = reader.ReadLine()) != null)
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
                yield return book;
            }
        }
    }
    
    private void OverwriteRootFile(string rootFilePath, string tempFilePath)
    {
        File.Delete(rootFilePath);
        File.Move(tempFilePath, rootFilePath);
    }
}