using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Models;

namespace Catalog.Services
{
    public interface IService<T>
        where T : IEntity
    {
        Task<T> CreateAsync(T item);

        Task DeleteAsync(string item);

        Task<List<T>> GetAllAsync();

        Task<T> GetByIdAsync(string id);

        Task UpdateAsync(string id, T itemIn);
    }
}
