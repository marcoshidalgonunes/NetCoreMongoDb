using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Core;
using Catalog.Models;

namespace Catalog.Services
{
    public abstract class BaseService<T> : IService<T>
        where T : IEntity
    {
        private readonly IRepository<T> _repository;

        protected BaseService(IRepository<T> repository)
        {
            _repository = repository;
        }

        public async Task<T> CreateAsync(T item)
        {
            await _repository.CreateAsync(item);
            return item;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var item = await GetByIdAsync(id);
            if (item == null)
            {
                return false;
            }

            await _repository.DeleteAsync(id);
            return true;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _repository.ReadAllAsync();
        }

        public async Task<List<T>> GetByCriteriaAsync(string criteria, string search)
        {
            return await _repository.ReadByCriteriaAsync(criteria, search);
        }

        public async Task<T> GetByIdAsync(string id)
        {
            return await _repository.ReadByIdAsync(id);
        }

        public async Task<bool> UpdateAsync(string id, T itemIn) 
        {
            var item = await GetByIdAsync(id);
            if (item == null || item.Id != itemIn.Id)
            {
                return false;
            }

            await _repository.UpdateAsync(id, itemIn);
            return true;
        }
            
    }
}
