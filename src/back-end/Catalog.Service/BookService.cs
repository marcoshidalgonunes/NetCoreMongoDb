using System.Threading.Tasks;
using Catalog.Data;
using Catalog.Data.MongoDb;
using Catalog.Domain.Entity;
using Catalog.Domain.Exceptions;
using Catalog.Domain.Validation;
using Catalog.Service.MongoDb;

namespace Catalog.Service
{
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
}
