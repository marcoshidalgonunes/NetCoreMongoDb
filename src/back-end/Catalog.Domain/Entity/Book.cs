namespace Catalog.Domain.Entity;

public sealed class Book : Entity<string>
{
    public override string Id { get; set; }

    public string Name { get; set; }

    public decimal Price { get; set; }

    public string Category { get; set; }

    public string Author { get; set; }
}
