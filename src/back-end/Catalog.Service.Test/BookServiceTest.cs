using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Data.MongoDb;
using Catalog.Domain.Entity;
using Moq;
using Xunit;

namespace Catalog.Service.Test
{
    public sealed class BookServiceTest
    {
        private readonly Mock<IMongoDbRepository<Book, string>> repositoryMock = new Mock<IMongoDbRepository<Book, string>>();

        [Fact]
        public async Task Create()
        {
            // Arrange
            Book book = new()
            {
                Author = "Ralph Johnson",
                Name = "Design Patterns",
                Category = "Computers",
                Price = 54.90M
            };
            repositoryMock
                .Setup(o => o.CreateAsync(book))
                .ReturnsAsync(new Book
                {
                    Id = "613260743633c438d5250513",
                    Author = book.Author,
                    Name = book.Name,
                    Category = book.Category,
                    Price = book.Price
                });
            var service = new BookService(repositoryMock.Object);

            // Act
            var item = await service.CreateAsync(book);

            // Assert
            Assert.True(item.Name == book.Name);
        }

        [Fact]
        public async Task GetAll()
        {
            // Arrange
            repositoryMock
                .Setup(o => o.ReadAllAsync())
                .ReturnsAsync(new List<Book> {
                    new Book {
                        Id = "613260743633c438d5250513",
                        Author = "Ralph Johnson",
                        Name = "Design Patterns",
                        Category = "Computers",
                        Price = 54.90M
                    },
                    new Book {
                        Id = "613260743633c438d5250514",
                        Author = "Robert C. Martin",
                        Name = "Clean Code",
                        Category = "Computers",
                        Price = 43.15M
                    }
                });
            var service = new BookService(repositoryMock.Object);

            // Act
            var items = await service.GetAllAsync();

            // Assert
            Assert.True(items.Count > 0);
        }

        [Theory]
        [InlineData("Name", "Design Patterns")]
        [InlineData("Category", "Computers")]
        public async Task GetByCriteria(string criteria, string search)
        {
            // Arrange
            repositoryMock
                .Setup(o => o.ReadByCriteriaAsync(criteria, search))
                .ReturnsAsync(new List<Book> {
                    new Book {
                        Id = "613260743633c438d5250513",
                        Author = "Ralph Johnson",
                        Name = "Design Patterns",
                        Category = "Computers",
                        Price = 54.90M
                    },
                    new Book {
                        Id = "613260743633c438d5250514",
                        Author = "Robert C. Martin",
                        Name = "Clean Code",
                        Category = "Computers",
                        Price = 43.15M
                    }
                });
            var service = new BookService(repositoryMock.Object);

            // Act
            var items = await service.GetByCriteriaAsync(criteria, search);

            // Assert
            Assert.True(items.Count > 0);
        }

        [Theory]
        [InlineData("Category", "Games")]
        [InlineData("Style", "Computers")]
        public async Task GetByCriteriaNotFound(string criteria, string search)
        {
            // Arrange
            repositoryMock
                .Setup(o => o.ReadByCriteriaAsync(criteria, search))
                .ReturnsAsync(value: null);
            var service = new BookService(repositoryMock.Object);

            // Act
            var items = await service.GetByCriteriaAsync(criteria, search);

            // Assert
            Assert.Null(items);
        }

        [Theory]
        [InlineData("613260743633c438d5250513")]
        public async Task GetById(string id)
        {
            // Arrange
            repositoryMock
                .Setup(o => o.ReadByIdAsync(id))
                .ReturnsAsync(new Book
                {
                    Id = id,
                    Author = "Ralph Johnson",
                    Name = "Design Patterns",
                    Category = "Computers",
                    Price = 54.90M
                });
            var service = new BookService(repositoryMock.Object);

            // Act
            var item = await service.GetByIdAsync(id);

            // Assert
            Assert.NotNull(item);
        }

        [Theory]
        [InlineData("613260743633c438d5250513")]
        public async Task GetByIdNotFound(string id)
        {
            // Arrange
            repositoryMock
                .Setup(o => o.ReadByIdAsync(id))
                .ReturnsAsync(value: null);
            var service = new BookService(repositoryMock.Object);

            // Act
            var item = await service.GetByIdAsync(id);

            // Assert
            Assert.Null(item);
        }

        [Fact]
        public async Task Update()
        {
            // Arrange
            Book book = new Book
            {
                Id = "613260743633c438d5250513",
                Author = "Ralph Johnson",
                Name = "Design Patterns",
                Category = "Computers",
                Price = 54.90M
            };
            repositoryMock.Setup(o => o.UpdateAsync(book));
            var service = new BookService(repositoryMock.Object);

            // Act
            var exception = await Record.ExceptionAsync(() => service.UpdateAsync(book));

            // Assert
            Assert.Null(exception);
        }

        [Theory]
        [InlineData("613260743633c438d5250513")]
        public async Task Delete(string id)
        {
            // Arrange
            repositoryMock.Setup(o => o.DeleteAsync(id));
            var service = new BookService(repositoryMock.Object);

            // Act
            var exception = await Record.ExceptionAsync(() => service.DeleteAsync(id));

            // Assert
            Assert.Null(exception);
        }
    }
}
