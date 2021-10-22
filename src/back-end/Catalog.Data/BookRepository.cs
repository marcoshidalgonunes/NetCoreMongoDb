using Catalog.Data.MongoDb;
using Catalog.Domain.Entity;

namespace Catalog.Data
{
    public sealed class BookRepository : BaseMongoDbRepository<Book, string>
    {
        public BookRepository(IMongoDbSettings settings)
            : base(settings) { }
    }
}
