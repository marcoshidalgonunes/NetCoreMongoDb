using Catalog.Domain.Models;
using Catalog.Repositories.MongoDb;

namespace Catalog.Repositories;

public sealed class BookRepository : BaseMongoDbRepository<Book, string>
{
    public BookRepository(IMongoDbSettings settings)
        : base(settings) { }
}
