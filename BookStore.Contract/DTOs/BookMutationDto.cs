namespace BookStore.Contract.DTOs;

public record BookMutationDto(
    string Title,
    string Description,
    string Author,
    int Year);