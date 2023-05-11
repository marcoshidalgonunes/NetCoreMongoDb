using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Domain.Entity;

namespace Catalog.Data;

public interface IRepository<TEntity, TIdentifier>
    where TEntity : Entity<TIdentifier>
{
    Task<TEntity> CreateAsync(TEntity item);

    Task<List<TEntity>> ReadAllAsync();

    Task<TEntity> ReadByIdAsync(TIdentifier id);

    Task UpdateAsync(TEntity itemIn);

    Task DeleteAsync(TIdentifier id);
}
