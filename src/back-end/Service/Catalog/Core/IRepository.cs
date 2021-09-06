using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Models;

namespace Catalog.Core
{
    public interface IRepository<T>
        where T : IEntity
    {
        Task<T> CreateAsync(T item);

        Task<List<T>> ReadAllAsync();

        Task<T> ReadByIdAsync(string id);

        Task UpdateAsync(string id, T itemIn);

        Task DeleteAsync(string id);
    }
}
