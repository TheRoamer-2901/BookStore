using System.ComponentModel.DataAnnotations;
namespace BookStore.Contract.DTOs;

public record BookMutationDto(
    [Required(ErrorMessage = "Title is required")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 100 characters")]
    string Title,
    
    string? Description,
    
    [Required(ErrorMessage = "Author is required")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Author must be between 3 and 100 characters")]
    string Author,

    [Range(1450, 2100, ErrorMessage = "Year must be between 1450 and 2100")]
    int Year
);