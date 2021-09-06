using Catalog.Core;
using Catalog.Models;

namespace Catalog.Services
{
    public sealed class BookService : ServiceBase<Book>
    {
        public BookService(IRepository<Book> repository)
            : base(repository) { }
    }
}
