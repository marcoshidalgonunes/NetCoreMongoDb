using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Data.MongoDb;
using Catalog.Domain.Entity;

namespace Catalog.Service.MongoDb;

public abstract class BaseMongoDbService<TEntity, TIdentifier> : IMongoDbService<TEntity, TIdentifier>
    where TEntity : IEntity<TIdentifier>
{
    private readonly IMongoDbRepository<TEntity, TIdentifier> _repository;

    protected BaseMongoDbService(IMongoDbRepository<TEntity, TIdentifier> repository)
    {
        _repository = repository;
    }

    protected abstract Task<TEntity> GetValidatedEntity(TEntity entity);

    public async Task<TEntity> CreateAsync(TEntity item)
    {
        await _repository.CreateAsync(await GetValidatedEntity(item));
        return item;
    }

    public async Task<bool> DeleteAsync(TIdentifier id)
    {
        var item = await GetByIdAsync(id);
        if (item == null)
        {
            return false;
        }

        await _repository.DeleteAsync(id);
        return true;
    }

    public async Task<List<TEntity>> GetAllAsync()
    {
        return await _repository.ReadAllAsync();
    }

    public async Task<List<TEntity>> GetByCriteriaAsync(string criteria, string search)
    {
        return await _repository.ReadByCriteriaAsync(criteria, search);
    }

    public async Task<TEntity> GetByIdAsync(TIdentifier id)
    {
        return await _repository.ReadByIdAsync(id);
    }

    public async Task<bool> UpdateAsync(TEntity itemIn) 
    {
        var item = await GetByIdAsync(itemIn.Id);
        if (item == null)
        {
            return false;
        }

        await _repository.UpdateAsync(await GetValidatedEntity(itemIn));
        return true;
    }            
}
