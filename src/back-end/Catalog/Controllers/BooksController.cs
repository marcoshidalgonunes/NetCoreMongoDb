﻿using Catalog.Domain.Models;
using Catalog.Services.MongoDb;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class BooksController(IMongoDbService<Book, string?> bookService) : ControllerBase
{
    private readonly IMongoDbService<Book, string?> _bookService = bookService;

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Book>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<Book>>> Get() =>
        await _bookService.GetAllAsync();

    [HttpGet("{id:length(24)}", Name = "GetBook")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Book))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Book>> Get(string id)
    {
        var book = await _bookService.GetByIdAsync(id);

        if (book == null)
        {
            return NotFound();
        }

        return book;
    }

    [HttpGet("{criteria}/{search}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Book>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Book))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Book>> Create(Book book)
    {
        await _bookService.CreateAsync(book);

        return CreatedAtRoute("GetBook", new { id = book.Id }, book);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
