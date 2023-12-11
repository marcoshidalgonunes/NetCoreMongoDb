using Catalog.Domain.Models;

namespace Catalog.Services;

public interface IService<TEntity, TIdentifier>
    where TEntity : Entity<TIdentifier>
{
    Task<TEntity> CreateAsync(TEntity item);

    Task<bool> DeleteAsync(TIdentifier item);

    Task<List<TEntity>> GetAllAsync();

    Task<TEntity> GetByIdAsync(TIdentifier id);

    Task<bool> UpdateAsync(TEntity itemIn);
}
