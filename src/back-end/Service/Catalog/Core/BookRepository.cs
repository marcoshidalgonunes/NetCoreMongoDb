using Catalog.Models;

namespace Catalog.Core
{
    public sealed class BookRepository : MongoDbBaseRepository<Book>
    {
        public BookRepository(ICatalogDatabaseSettings settings)
            : base(settings, settings.BooksCollectionName) { }
    }
}
