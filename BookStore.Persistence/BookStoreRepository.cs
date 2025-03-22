using BookStore.Domain;

namespace BookStore.Persistence;

public class BookStoreRepository : IBookStoreRepository
{
    public void Add(Book book)
    {
        throw new NotImplementedException();
    }

    public void Update(Book book)
    {
        throw new NotImplementedException();
    }

    public void Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    public Book? GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public List<Book> GetAll()
    {
        throw new NotImplementedException();
    }
}