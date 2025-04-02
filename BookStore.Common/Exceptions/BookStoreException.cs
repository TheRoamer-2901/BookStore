namespace BookStore.Common.Exceptions;

public class BookStoreException : Exception
{
    public BookStoreException(string message) : base(message)
    {
    }
}