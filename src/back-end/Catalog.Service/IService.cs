using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Domain.Entity;

namespace Catalog.Service
{
    public interface IService<TEntity, TIdentifier>
        where TEntity : IEntity<TIdentifier>
    {
        Task<TEntity> CreateAsync(TEntity item);

        Task<bool> DeleteAsync(TIdentifier item);

        Task<List<TEntity>> GetAllAsync();

        Task<TEntity> GetByIdAsync(TIdentifier id);

        Task<bool> UpdateAsync(TEntity itemIn);
    }
}
