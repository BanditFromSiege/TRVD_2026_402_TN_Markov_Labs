using Microsoft.AspNetCore.Mvc;
using SimpleLibrary.Application.Services;
using SimpleLibrary.Infrastructure.Models;
using SimpleLibrary.REST.Models;

namespace SimpleLibrary.REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        // GET: api/books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookResponseModel>>> GetAll()
        {
            var books = await _bookService.GetAllBooksAsync();

            var result = books.Select(MapToResponse);

            return Ok(result);
        }

        // GET: api/books/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<BookResponseModel>> GetById(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);

            if (book == null)
                return NotFound();

            return Ok(MapToResponse(book));
        }

        // POST: api/books
        [HttpPost]
        public async Task<ActionResult<BookResponseModel>> Create([FromBody] BookCreateModel model)
        {
            var book = new Book
            {
                Title = model.Title,
                Author = model.Author,
                Isbn = model.ISBN,
                Genre = model.Genre,
                TotalCopies = model.TotalCopies,
                AvailableCopies = model.TotalCopies
            };

            var created = await _bookService.CreateBookAsync(book);

            return CreatedAtAction(
                nameof(GetById),
                new { id = created.Id },
                MapToResponse(created)
            );
        }

        // PUT: api/books/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<BookResponseModel>> Update(int id, [FromBody] BookUpdateModel model)
        {
            var book = new Book
            {
                Title = model.Title,
                Author = model.Author,
                Isbn = model.ISBN,
                Genre = model.Genre,
                TotalCopies = model.TotalCopies
            };

            var updated = await _bookService.UpdateBookAsync(id, book);

            if (updated == null)
                return NotFound();

            return Ok(MapToResponse(updated));
        }

        // DELETE: api/books/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _bookService.DeleteBookAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }

        private static BookResponseModel MapToResponse(Book book)
        {
            return new BookResponseModel
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                ISBN = book.Isbn ?? string.Empty,
                Genre = book.Genre ?? string.Empty,
                AvailableCopies = book.AvailableCopies
            };
        }
    }
}