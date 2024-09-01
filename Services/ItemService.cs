using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDbDemo.Models;

namespace MongoDbDemo.Services
{
    public class ItemService
    {
        private readonly IMongoCollection<Item> _itemsCollection;

        public ItemService(IMongoDatabase database)
        {
            _itemsCollection = database.GetCollection<Item>("Items");
        }

        public async Task<List<Item>> GetAsync() =>
            await _itemsCollection.Find(item => true).ToListAsync();

        public async Task<Item> GetAsync(string id) =>
            await _itemsCollection.Find<Item>(item => item.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Item newItem) =>
            await _itemsCollection.InsertOneAsync(newItem);

        public async Task UpdateAsync(string id, Item updatedItem) =>
            await _itemsCollection.ReplaceOneAsync(item => item.Id == id, updatedItem);

        public async Task RemoveAsync(string id) =>
            await _itemsCollection.DeleteOneAsync(item => item.Id == id);
    }
}
