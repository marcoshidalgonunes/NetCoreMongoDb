using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Controllers;
using Catalog.Domain.Models;
using Catalog.Services.MongoDb;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Catalog.Test.Controllers;

public sealed class BooksControllerTests
{
    private readonly Mock<IMongoDbService<Book, string>> serviceMock = new Mock<IMongoDbService<Book, string>>();

    [Fact]
    public async Task Create()
    {
        // Arrange
        string id = "613260743633c438d5250513";
        Book book = new()
        {
            Id = id,
            Author = "Ralph Johnson",
            Name = "Design Patterns",
            Category = "Computers",
            Price = 54.90
        };

        serviceMock
            .Setup(s => s.CreateAsync(book))
            .ReturnsAsync(new Book
            {
                Id = id,
                Author = book.Author,
                Name = book.Name,
                Category = book.Category,
                Price = book.Price
            });
        var controller = new BooksController(serviceMock.Object);

        // Act
        var result = await controller.Create(book);

        // Assert
        var returnValue = Assert.IsType<ActionResult<Book>>(result);
        Assert.IsType<CreatedAtRouteResult>(returnValue.Result);
    }

    [Fact]
    public async Task Get()
    {
        // Arrange
        serviceMock
            .Setup(s => s.GetAllAsync())
            .ReturnsAsync(new List<Book> {
                new Book {
                    Id = "613260743633c438d5250513",
                    Author = "Ralph Johnson",
                    Name = "Design Patterns",
                    Category = "Computers",
                    Price = 54.90
                },
                new Book {
                    Id = "613260743633c438d5250514",
                    Author = "Robert C. Martin",
                    Name = "Clean Code",
                    Category = "Computers",
                    Price = 43.15
                }
            });
        var controller = new BooksController(serviceMock.Object);

        // Act
        var result = await controller.Get();

        // Assert
        var returnValue = Assert.IsType<List<Book>>(result.Value);
        Assert.True(returnValue.Count > 0);
    }

    [Theory]
    [InlineData("613260743633c438d5250513")]
    public async Task GetById(string id)
    {
        // Arrange
        serviceMock
            .Setup(s => s.GetByIdAsync(id))
            .ReturnsAsync(new Book
            {
                Id = id,
                Author = "Ralph Johnson",
                Name = "Design Patterns",
                Category = "Computers",
                Price = 54.90
            });
        var controller = new BooksController(serviceMock.Object);

        // Act
        var result = await controller.Get(id);

        // Assert
        var returnValue = Assert.IsType<Book>(result.Value);
        Assert.NotNull(returnValue);
    }

    [Theory]
    [InlineData("313260743633c438d5250513")]
    public async Task GetByIdNotFound(string id)
    {
        // Arrange
        serviceMock
            .Setup(s => s.GetByIdAsync(id))
            .ReturnsAsync(value: null);
        var controller = new BooksController(serviceMock.Object);

        // Act
        var result = await controller.Get(id);

        // Assert
        var returnValue = Assert.IsType<ActionResult<Book>>(result);
        Assert.IsType<NotFoundResult>(returnValue.Result);
    }

    [Theory]
    [InlineData("Name", "Design Patterns")]
    [InlineData("Category", "Computers")]
    public async Task Search(string criteria, string search)
    {
        // Arrange
        serviceMock
            .Setup(s => s.GetByCriteriaAsync(criteria, search))
            .ReturnsAsync(new List<Book> {
                new Book {
                    Id = "613260743633c438d5250513",
                    Author = "Ralph Johnson",
                    Name = "Design Patterns",
                    Category = "Computers",
                    Price = 54.90
                },
                new Book {
                    Id = "613260743633c438d5250514",
                    Author = "Robert C. Martin",
                    Name = "Clean Code",
                    Category = "Computers",
                    Price = 43.15
                }
            });
        var controller = new BooksController(serviceMock.Object);

        // Act
        var result = await controller.Get(criteria, search);

        // Assert
        var returnValue = Assert.IsType<List<Book>>(result.Value);
        Assert.True(returnValue.Count > 0);
    }

    [Theory]
    [InlineData("Category", "Games")]
    [InlineData("Style", "Computers")]
    public async Task SearchNotFound(string criteria, string search)
    {
        // Arrange
        serviceMock
            .Setup(s => s.GetByCriteriaAsync(criteria, search))
            .ReturnsAsync(value: null);
        var service = new BooksController(serviceMock.Object);

        // Act
        var result = await service.Get(criteria, search);

        // Assert
        var returnValue = Assert.IsType<ActionResult<List<Book>>>(result);
        Assert.IsType<NotFoundResult>(returnValue.Result);
    }

    [Fact]
    public async Task Update()
    {
        // Arrange
        Book book = new()
        {
            Id = "613260743633c438d5250513",
            Author = "Ralph Johnson",
            Name = "Design Patterns",
            Category = "Computers",
            Price = 54.90
        };

        serviceMock
            .Setup(s => s.UpdateAsync(book))
            .ReturnsAsync(true);
        var controller = new BooksController(serviceMock.Object);

        // Act
        var result = await controller.Update(book);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task UpdateNotFound()
    {
        // Arrange
        Book book = new()
        {
            Id = "313260743633c438d5250529",
            Author = "Ralph Johnson",
            Name = "Design Patterns",
            Category = "Computers",
            Price = 54.90
        };

        serviceMock
            .Setup(s => s.UpdateAsync(book))
            .ReturnsAsync(false);

        var controller = new BooksController(serviceMock.Object);

        // Act
        var result = await controller.Update(book);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Theory]
    [InlineData("613260743633c438d5250513")]
    public async Task Delete(string id)
    {
        // Arrange
        serviceMock
            .Setup(s => s.DeleteAsync(id))
            .ReturnsAsync(true);

        var controller = new BooksController(serviceMock.Object);

        // Act
        var result = await controller.Delete(id);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Theory]
    [InlineData("313260743633c438d5250513")]
    public async Task DeleteNotFound(string id)
    {
        // Arrange
        serviceMock
            .Setup(s => s.DeleteAsync(id))
            .ReturnsAsync(false);
        var controller = new BooksController(serviceMock.Object);

        // Act
        var result = await controller.Delete(id);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
