using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Core;
using Catalog.Models;

namespace Catalog.Services
{
    public abstract class ServiceBase<T> : IService<T>
        where T : IEntity
    {
        private readonly IRepository<T> _repository;

        protected ServiceBase(IRepository<T> repository)
        {
            _repository = repository;
        }

        public async Task<T> CreateAsync(T item)
        {
            await _repository.CreateAsync(item);
            return item;
        }

        public async Task DeleteAsync(string id) =>
            await _repository.DeleteAsync(id);

        public async Task<List<T>> GetAllAsync()
        {
            return await _repository.ReadAllAsync();
        }

        public async Task<T> GetByIdAsync(string id)
        {
            return await _repository.ReadByIdAsync(id);
        }

        public async Task UpdateAsync(string id, T itemIn) =>
            await _repository.UpdateAsync(id, itemIn);
    }
}
