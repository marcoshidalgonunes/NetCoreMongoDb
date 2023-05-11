namespace Catalog.Domain.Entity;

public abstract class Entity<TIdentifier>
{ 
    public abstract TIdentifier Id { get; set; }
}
