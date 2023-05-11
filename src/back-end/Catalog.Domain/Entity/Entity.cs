namespace Catalog.Domain.Entity;

public abstract class Entity<TIdentifier> : IEntity<TIdentifier>
{
    public abstract TIdentifier Id { get; set; }
}
