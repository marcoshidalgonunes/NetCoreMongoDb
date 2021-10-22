using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Catalog.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.Core
{
    public abstract class BaseMongoDbRepository<T> : IRepository<T>
        where T : IEntity
    {
        private readonly IMongoCollection<T> _collection;

        protected BaseMongoDbRepository(ICatalogDatabaseSettings settings, string collectionName)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _collection = database.GetCollection<T>(collectionName);
        }

        public async Task<T> CreateAsync(T item)
        {
            await _collection.InsertOneAsync(item);
            return item;
        }

        public async Task<List<T>> ReadAllAsync()
        {
            var items = await _collection.FindAsync(item => true);

            return items.ToList();
        }

        public async Task<List<T>> ReadByCriteriaAsync(string criteria, string search)
        {
            var queryExpr = new BsonRegularExpression(new Regex(search, RegexOptions.IgnoreCase));
            var builder = Builders<T>.Filter;
            var filter = builder.Regex(criteria, queryExpr);

            var items = await _collection.Find(filter).ToListAsync();

            return items.ToList();
        }

        public async Task<T> ReadByIdAsync(string id)
        {
            var items = await _collection.FindAsync(item => item.Id == id);

            return items.FirstOrDefault();
        }

        public async Task UpdateAsync(T itemIn) =>
            await _collection.ReplaceOneAsync(item => item.Id == itemIn.Id, itemIn);

        public async Task DeleteAsync(string id) =>
            await _collection.DeleteOneAsync(item => item.Id == id);
    }
}
