using System.Text.Json;
using BookStore.Domain;

namespace BookStore.Persistence;

public class BookStoreRepository : IBookStoreRepository
{
    private const string FilePath = @"D:\projects\BookStore\BookStore.Persistence\books.json";

    public BookStoreRepository()
    {
    }

    public void Add(Book book)
    {
        throw new NotImplementedException();
    }

    public void Update(Book updatedBook)
    {
        string tempFile = FilePath.Replace(".json", ".tmp.json");
        bool updated = false;
        using var writer = new StreamWriter(tempFile);

        writer.WriteLine("[");
        foreach (var book in LoadBooks())
        {
            var bookToWrite = book.Id == updatedBook.Id ? updatedBook : book;
            writer.WriteLine($"    {JsonSerializer.Serialize(bookToWrite)},");
        
            if (book.Id == updatedBook.Id)
            {
                updated = true;
            }
        }
        writer.WriteLine("]");

        if (!updated)
        {
            throw new InvalidOperationException("Book not found.");
        }
        writer.Close();
        File.Delete(FilePath);
        File.Move(tempFile, FilePath);
    }

    public void Delete(Guid id)
    {
        throw new NotImplementedException();
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

    public IEnumerable<Book> LoadBooks()
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
}