using Catalog.Domain.Models;
using Catalog.Repositories.MongoDb;

namespace Catalog.Repositories;

public sealed class BookRepository(IMongoDbSettings settings) : BaseMongoDbRepository<Book, string?>(settings)
{
}
