
using ColletteAPI.Models.Domain;
using ColletteAPI.Repositories;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ColletteAPI.Data
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly IMongoCollection<Inventory> _inventories;

        public InventoryRepository(IMongoClient client, IConfiguration configuration)
        {
            var database = client.GetDatabase(configuration.GetSection("MongoDB:DatabaseName").Value);
            _inventories = database.GetCollection<Inventory>("Inventories");
        }

        public async Task<IEnumerable<Inventory>> GetAllInventoriesAsync()
        {
            return await _inventories.Find(Builders<Inventory>.Filter.Empty).ToListAsync();
        }

        public async Task<Inventory> GetInventoryByIdAsync(int inventoryId)
        {
            var filter = Builders<Inventory>.Filter.Eq(i => i.InventoryId, inventoryId);
            return await _inventories.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<Inventory> GetInventoryByProductIdAsync(int productId)
        {
            var filter = Builders<Inventory>.Filter.Eq(i => i.ProductId, productId);
            return await _inventories.Find(filter).FirstOrDefaultAsync();
        }

        public async Task AddInventoryAsync(Inventory inventory)
        {
            await _inventories.InsertOneAsync(inventory);
        }

        public async Task UpdateInventoryAsync(Inventory inventory)
        {
            var filter = Builders<Inventory>.Filter.Eq(i => i.InventoryId, inventory.InventoryId);
            await _inventories.ReplaceOneAsync(filter, inventory);
        }

        public async Task RemoveInventoryAsync(int inventoryId, string orderStatus)
        {
            var filter = Builders<Inventory>.Filter.Eq(i => i.InventoryId, inventoryId);
            var inventory = await _inventories.Find(filter).FirstOrDefaultAsync();

            // Only allow deletion for certain order statuses
            if (inventory != null && (orderStatus == "Purchased" || orderStatus == "Accepted" || orderStatus == "Processing"))
            {
                await _inventories.DeleteOneAsync(filter);
            }
            else
            {
                throw new InvalidOperationException("Inventory cannot be removed for Delivered or Cancelled orders.");
            }
        }

        public async Task<bool> InventoryExistsAsync(int productId)
        {
            var filter = Builders<Inventory>.Filter.Eq(i => i.ProductId, productId);
            return await _inventories.Find(filter).AnyAsync();
        }
    }
}
