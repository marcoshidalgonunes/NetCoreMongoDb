using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Core;
using Catalog.Models;
using Catalog.Services;
using Moq;
using Xunit;

namespace Catalog.Tests
{
    public sealed class BookServiceTest 
    {
        private readonly Mock<IRepository<Book>> repositoryMock = new Mock<IRepository<Book>>();

        [Fact]
        public async Task Create()
        {
            // Arrange
            Book book = new()
            {
                Author = "Ralph Johnson",
                BookName = "Design Patterns",
                Category = "Computers",
                Price = 54.90M
            };
            repositoryMock
                .Setup(o => o.CreateAsync(book))
                .ReturnsAsync(new Book
                {
                    Id = "613260743633c438d5250513",
                    Author = book.Author,
                    BookName = book.BookName,
                    Category = book.Category,
                    Price = book.Price
                });
            var service = new BookService(repositoryMock.Object);

            // Act
            var item = await service.CreateAsync(book);

            // Assert
            Assert.True(item.BookName == book.BookName);
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
                        BookName = "Design Patterns", 
                        Category = "Computers", 
                        Price = 54.90M 
                    },
                    new Book {
                        Id = "613260743633c438d5250514",
                        Author = "Robert C. Martin",
                        BookName = "Clean Code",
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
                        BookName = "Design Patterns",
                        Category = "Computers",
                        Price = 54.90M
                    },
                    new Book {
                        Id = "613260743633c438d5250514",
                        Author = "Robert C. Martin",
                        BookName = "Clean Code",
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
                    BookName = "Design Patterns",
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
            repositoryMock.Setup(o => o.UpdateAsync(id, book));
            var service = new BookService(repositoryMock.Object);

            // Act
            var exception = await Record.ExceptionAsync(() => service.UpdateAsync(id, book));

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
