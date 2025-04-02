namespace BookStore.Common.Exceptions;

public class BookDuplicatedException : BookStoreException
{
    public Guid Id { get; }

    public BookDuplicatedException(Guid id)
        : base($"Book with id {id} is duplicated")
    {
        Id = id;
    }
}