using Catalog.Domain.Exceptions;
using Catalog.Domain.Models;
using Catalog.Domain.Validation;
using Catalog.Repositories.MongoDb;
using Catalog.Services.MongoDb;

namespace Catalog.Services;

public sealed class BookService : BaseMongoDbService<Book, string>
{
    public BookService(IMongoDbRepository<Book, string> repository)
        : base(repository) { }

    protected async override Task<Book> GetValidatedEntity(Book book)
    {
        var validator = new BookValidator();
        var validatorResult = await validator.ValidateAsync(book);

        if (validatorResult.Errors.Count > 0)
            throw new ValidationException(validatorResult);

        return book;
    }
}
