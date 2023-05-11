using Catalog.Data.MongoDb;
using Catalog.Domain.Entity;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xunit;

namespace Catalog.Data.Test;

public sealed class BookRepositoryTest : IClassFixture<Fixture.DependencyInjectionFixture>
{
    private readonly ServiceProvider _serviceProvider;

    private IMongoDbSettings Settings
    {
        get { return _serviceProvider.GetService<MongoDbSettings>(); }
    }

    public BookRepositoryTest(Fixture.DependencyInjectionFixture fixture)
    {
        _serviceProvider = fixture.ServiceProvider;
    }

    [Fact]
    public async Task Create()
    {
        // Arrange
        Book book = new()
        {
            Author = "Jeff Atwood",
            Name = "Effective Programming: More Than Writing Code",
            Category = "Programming",
            Price = 61.90
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
    [InlineData("Name", "Design Patterns")]
    [InlineData("Category", "Computers")]
    public async Task ReadByCriteria(string criteria, string search)
    {
        // Arrange
        var bookRepository = new BookRepository(Settings);

        // Act
        var items = await bookRepository.ReadByCriteriaAsync(criteria, search);

        // Assert
        Assert.True(items.Count > 0);
    }

    [Theory]
    [InlineData("Category", "Games")]
    [InlineData("Style", "Computers")]
    public async Task ReadByCriteriaNotFound(string criteria, string search)
    {
        // Arrange
        var bookRepository = new BookRepository(Settings);

        // Act
        var items = await bookRepository.ReadByCriteriaAsync(criteria, search);

        // Assert
        Assert.False(items.Count > 0);
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
        var bookRepository = new BookRepository(Settings);

        // Act
        var exception = await Record.ExceptionAsync(() => bookRepository.UpdateAsync(book));

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
