using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Models;
using Catalog.Services;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class BooksController : ControllerBase
    {
        private readonly IService<Book> _bookService;

        public BooksController(IService<Book> bookService)
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

        [HttpPost]
        public async Task<ActionResult<Book>> Create(Book book)
        {
            await _bookService.CreateAsync(book);

            return CreatedAtRoute("GetBook", new { id = book.Id.ToString() }, book);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Book bookIn)
        {
            var book = await _bookService.GetByIdAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            await _bookService.UpdateAsync(id, bookIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var book = await _bookService.GetByIdAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            await _bookService.DeleteAsync(book.Id);

            return NoContent();
        }
    }
}
