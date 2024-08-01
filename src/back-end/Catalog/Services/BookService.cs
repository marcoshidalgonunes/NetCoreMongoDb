using Catalog.Domain.Models;
using Catalog.Domain.Validation;
using Catalog.Repositories.MongoDb;
using Catalog.Services.MongoDb;
using FluentValidation;

namespace Catalog.Services;

public sealed class BookService(IMongoDbRepository<Book, string?> repository) : BaseMongoDbService<Book, string?>(repository)
{
    private static readonly IValidator<Book> _validator = new BookValidator();

    protected async override Task<Book> GetValidatedEntity(Book book)
    {
        await _validator.ValidateAndThrowAsync(book);
        return book;
    }
}
