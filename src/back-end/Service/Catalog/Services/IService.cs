using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Models;

namespace Catalog.Services
{
    public interface IService<T>
        where T : IEntity
    {
        Task<T> CreateAsync(T item);

        Task<bool> DeleteAsync(string item);

        Task<List<T>> GetAllAsync();

        Task<List<T>> GetByCriteriaAsync(string criteria, string search);

        Task<T> GetByIdAsync(string id);

        Task<bool> UpdateAsync(string id, T itemIn);
    }
}
