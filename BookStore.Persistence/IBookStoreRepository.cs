using BookStore.Domain;

namespace BookStore.Persistence;

public interface IBookStoreRepository
{
    void Add(Book book);
    void Update(Book book);
    void Delete(Guid id);
    Book? GetById(Guid id);
    List<Book> GetAll();
}