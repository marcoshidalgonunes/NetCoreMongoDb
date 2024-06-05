using Catalog.Domain.Models;

namespace Catalog.Services.MongoDb;

public interface IMongoDbService<TEntity, TIdentifier> : IService<TEntity, TIdentifier>
    where TEntity : Entity<TIdentifier>
{
    Task<List<TEntity>> GetByCriteriaAsync(string criteria, string search);
}
