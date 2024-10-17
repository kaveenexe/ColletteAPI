// InventoryRepository.cs
using ColletteAPI.Models.Domain;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace ColletteAPI.Repositories
{
    // Implementation of inventory repository
    public class InventoryRepository : IInventoryRepository
    {
        private readonly IMongoCollection<Inventory> _inventoryCollection;

        public InventoryRepository(IMongoDatabase database)
        {
            _inventoryCollection = database.GetCollection<Inventory>("Inventories");
        }

        // Retrieves inventory based on productId
        public async Task<Inventory> GetInventoryByProductIdAsync(string productId)
        {
            return await _inventoryCollection.Find(i => i.ProductId == productId).FirstOrDefaultAsync();
        }

        // Adds a new inventory item
        public async Task CreateInventoryAsync(Inventory inventory)
        {
            await _inventoryCollection.InsertOneAsync(inventory);
        }

        // Updates existing inventory item
        public async Task UpdateInventoryAsync(Inventory inventory)
        {
            await _inventoryCollection.ReplaceOneAsync(i => i.ProductId == inventory.ProductId, inventory);
        }

        // Retrieves all inventory items
        public async Task<IEnumerable<Inventory>> GetAllInventoriesAsync()
        {
            return await _inventoryCollection.Find(_ => true).ToListAsync();
        }
    }
}
