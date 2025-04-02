using AutoMapper;
using BookStore.Contract;
using BookStore.Contract.DTOs;
using BookStore.Domain;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookStoreManager _bookStoreManager;
    private readonly IMapper _mapper;

    public BooksController(IBookStoreManager bookStoreManager, IMapper mapper)
    {
        _bookStoreManager = bookStoreManager;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var books = await _bookStoreManager.GetAllBooksAsync(cancellationToken);
        var bookDtos = _mapper.Map<List<BookDto>>(books);
        return Ok(bookDtos);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var book = await _bookStoreManager.GetBookByIdAsync(id, cancellationToken);
        if (book is null)
            return NotFound();

        var bookDto = _mapper.Map<BookDto>(book);
        return Ok(bookDto);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BookMutationDto bookDto, CancellationToken cancellationToken)
    {
        var book = _mapper.Map<Book>(bookDto);
        await _bookStoreManager.AddBookAsync(book, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = book.Id }, _mapper.Map<BookDto>(book));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] BookMutationDto bookDto, CancellationToken cancellationToken)
    {
        var bookToUpdate = _mapper.Map<Book>(bookDto);
        bookToUpdate.Id = id;
        await _bookStoreManager.UpdateBookAsync(bookToUpdate, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _bookStoreManager.DeleteBookAsync(id, cancellationToken);
        return NoContent();
    }
}
