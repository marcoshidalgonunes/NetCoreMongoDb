using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Domain.Entity;

namespace Catalog.Data.MongoDb;

public interface IMongoDbRepository<TEntity, TIdentifier> : IRepository<TEntity, TIdentifier>
    where TEntity : IEntity<TIdentifier>
{
    Task<List<TEntity>> ReadByCriteriaAsync(string criteria, string search);
}
