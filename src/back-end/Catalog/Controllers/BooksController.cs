using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Domain.Entity;
using Catalog.Service.MongoDb;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class BooksController : ControllerBase
    {
        private readonly IMongoDbService<Book, string> _bookService;

        public BooksController(IMongoDbService<Book, string> bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Book>>> Get() =>
            await _bookService.GetAllAsync();

        [HttpGet("{id:length(24)}", Name = "GetBook")]
        public async Task<ActionResult<Book>> Get(string id)
        {
            var book = await _bookService.GetByIdAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        [HttpGet("{criteria},{search}")]
        public async Task<ActionResult<List<Book>>> Get(string criteria, string search)
        {
            var books = await _bookService.GetByCriteriaAsync(criteria, search);

            if (books == null || books.Count == 0)
            {
                return NotFound();
            }

            return books;
        }

        [HttpPost]
        public async Task<ActionResult<Book>> Create(Book book)
        {
            await _bookService.CreateAsync(book);

            return CreatedAtRoute("GetBook", new { id = book.Id }, book);
        }

        [HttpPut]
        public async Task<IActionResult> Update(Book bookIn)
        {
            bool updated = await _bookService.UpdateAsync(bookIn);
            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _bookService.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
