namespace BookStore.Common.Exceptions;

public class BookNotFoundException : BookStoreException
{
    public Guid Id { get; }

    public BookNotFoundException(Guid id)
        : base($"Book with id {id} is not found")
    {
        Id = id;
    }
}