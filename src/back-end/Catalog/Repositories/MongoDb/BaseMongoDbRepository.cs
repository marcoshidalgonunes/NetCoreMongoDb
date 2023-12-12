using System.Text.RegularExpressions;
using Catalog.Domain.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.Repositories.MongoDb;

public abstract class BaseMongoDbRepository<TEntity, TIdentifier> : IMongoDbRepository<TEntity, TIdentifier>
    where TEntity : Entity<TIdentifier>
{
    private readonly IMongoCollection<TEntity> _collection;

    protected BaseMongoDbRepository(IMongoDbSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase(settings.DatabaseName);

        _collection = database.GetCollection<TEntity>(settings.CollectionName);
    }

    public async Task<TEntity> CreateAsync(TEntity item)
    {
        await _collection.InsertOneAsync(item);
        return item;
    }

    public async Task<List<TEntity>> ReadAllAsync()
    {
        var items = await _collection.FindAsync(item => true);

        return items.ToList();
    }

    public async Task<List<TEntity>> ReadByCriteriaAsync(string criteria, string search)
    {
        var queryExpr = new BsonRegularExpression(new Regex(search, RegexOptions.IgnoreCase));
        var builder = Builders<TEntity>.Filter;
        var filter = builder.Regex(criteria, queryExpr);

        var items = await _collection.Find(filter).ToListAsync();

        return [.. items];
    }

    public async Task<TEntity> ReadByIdAsync(TIdentifier id)
    {
        using var items = await _collection.FindAsync(item => item.Id!.Equals(id));

        return items.FirstOrDefault();
    }

    public async Task UpdateAsync(TEntity itemIn) =>
        await _collection.ReplaceOneAsync(item => item.Id!.Equals(itemIn.Id), itemIn);

    public async Task DeleteAsync(TIdentifier id) =>
        await _collection.DeleteOneAsync(item => item.Id!.Equals(id));
}
