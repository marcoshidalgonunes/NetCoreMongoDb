namespace Catalog.Domain.Entity
{
    public interface IEntity<TIdentifier>
    {
        TIdentifier Id { get; set; }
    }
}
