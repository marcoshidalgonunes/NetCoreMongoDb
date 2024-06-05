using Catalog.Domain.Models;

namespace Catalog.Repositories.MongoDb;

public interface IMongoDbRepository<TEntity, TIdentifier> : IRepository<TEntity, TIdentifier>
    where TEntity : Entity<TIdentifier>
{
    Task<List<TEntity>> ReadByCriteriaAsync(string criteria, string search);
}
