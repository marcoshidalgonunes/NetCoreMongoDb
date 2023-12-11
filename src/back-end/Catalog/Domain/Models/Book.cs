﻿namespace Catalog.Domain.Models;

public sealed class Book : Entity<string>
{
    public override string Id { get; set; }

    public string Name { get; set; }

    public double Price { get; set; }

    public string Category { get; set; }

    public string Author { get; set; }
}
