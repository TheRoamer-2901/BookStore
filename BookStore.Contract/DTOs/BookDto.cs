namespace BookStore.Contract.DTOs;

public record BookDto(
    Guid Id,
    string Title,
    string Description,
    string Author,
    int Year);
