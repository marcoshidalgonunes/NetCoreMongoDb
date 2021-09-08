using System.Threading.Tasks;
using Catalog.Core;
using Catalog.Models;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Catalog.Tests
{
    public sealed class BookRepositoryTest : IClassFixture<DependencyInjectionFixture>
    {
        private readonly ServiceProvider _serviceProvider;

        private ICatalogDatabaseSettings Settings
        {
            get {  return _serviceProvider.GetService<ICatalogDatabaseSettings>(); }
        }

        public BookRepositoryTest(DependencyInjectionFixture fixture)
        {
            _serviceProvider = fixture.ServiceProvider;
        }

        [Fact]
        public async Task Create()
        {
            // Arrange
            Book book = new Book
            {
                Author = "Effective Programming: More Than Writing Code",
                BookName = "Jeff Atwood",
                Category = "Computers",
                Price = 61.90M
            };
            var bookRepository = new BookRepository(Settings);

            // Act
            var newBook = await bookRepository.CreateAsync(book);

            // Assert
            Assert.NotNull(newBook);
        }

        [Fact]
        public async Task ReadAll()
        {
            // Arrange
            var bookRepository = new BookRepository(Settings);

            // Act
            var items = await bookRepository.ReadAllAsync();

            // Assert
            Assert.True(items.Count > 0);
        }

        [Theory]
        [InlineData("613260743633c438d5250513")]
        public async Task ReadById(string id)
        {
            // Arrange
            var bookRepository = new BookRepository(Settings);

            // Act
            var book = await bookRepository.ReadByIdAsync(id);

            // Assert
            Assert.NotNull(book);
        }

        [Theory]
        [InlineData("313260743633c438d5250513")]
        public async Task ReadByIdNotFound(string id)
        {
            // Arrange
            var bookRepository = new BookRepository(Settings);

            // Act
            var book = await bookRepository.ReadByIdAsync(id);

            // Assert
            Assert.Null(book);
        }

        [Theory]
        [InlineData("613260743633c438d5250513")]
        public async Task Update(string id)
        {
            // Arrange
            Book book = new Book
            {
                Id = id,
                Author = "Ralph Johnson",
                BookName = "Design Patterns",
                Category = "Computers",
                Price = 54.90M
            };
            var bookRepository = new BookRepository(Settings);

            // Act
            var exception =  await Record.ExceptionAsync(() => bookRepository.UpdateAsync(id, book));

            // Assert
            Assert.Null(exception);
        }

        [Theory]
        [InlineData("61365ce59d442be4b420ccc3")]
        public async Task Delete(string id)
        {
            // Arrange
            var bookRepository = new BookRepository(Settings);

            // Act
            var exception = await Record.ExceptionAsync(() => bookRepository.DeleteAsync(id));

            // Assert
            Assert.Null(exception);
        }
    }
}
