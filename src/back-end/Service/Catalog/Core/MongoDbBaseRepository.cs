using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Models;
using MongoDB.Driver;

namespace Catalog.Core
{
    public abstract class MongoDbBaseRepository<T> : IRepository<T>
        where T : IEntity
    {
        private readonly IMongoCollection<T> _collection;

        protected MongoDbBaseRepository(ICatalogDatabaseSettings settings, string collectionName)
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

        public async Task<T> ReadByIdAsync(string id)
        {
            var items = await _collection.FindAsync(item => item.Id == id);

            return items.FirstOrDefault();
        }

        public async Task UpdateAsync(string id, T itemIn) =>
            await _collection.ReplaceOneAsync(item => item.Id == id, itemIn);

        public async Task DeleteAsync(string id) =>
            await _collection.DeleteOneAsync(item => item.Id == id);
    }
}
