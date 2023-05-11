using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Domain.Entity;

namespace Catalog.Service.MongoDb;

public interface IMongoDbService<TEntity, TIdentifier> : IService<TEntity, TIdentifier>
    where TEntity : Entity<TIdentifier>
{
    Task<List<TEntity>> GetByCriteriaAsync(string criteria, string search);
}
