// InventoryRepository.cs
using MongoDB.Driver;
using ColletteAPI.Models.Domain;

namespace ColletteAPI.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly IMongoCollection<Inventory> _inventories;

        public InventoryRepository(IMongoDatabase database)
        {
            _inventories = database.GetCollection<Inventory>("Inventories");
        }

        // Fetch all inventories
        public async Task<IEnumerable<Inventory>> GetAllInventoriesAsync()
        {
            return await _inventories.Find(inv => true).ToListAsync();
        }

        // Fetch inventory by ProductId
        public async Task<Inventory> GetInventoryByProductIdAsync(string productId)
        {
            return await _inventories.Find(inv => inv.ProductId == productId).FirstOrDefaultAsync();
        }

        // Add new inventory entry
        public async Task<Inventory> AddInventoryAsync(Inventory inventory)
        {
            await _inventories.InsertOneAsync(inventory);
            return inventory;
        }

        // Update existing inventory entry
        public async Task<bool> UpdateInventoryAsync(Inventory inventory)
        {
            var result = await _inventories.ReplaceOneAsync(inv => inv.Id == inventory.Id, inventory);
            return result.ModifiedCount > 0;
        }

        // Delete inventory entry
        public async Task<bool> DeleteInventoryAsync(string id)
        {
            var result = await _inventories.DeleteOneAsync(inv => inv.Id == id);
            return result.DeletedCount > 0;
        }
    }
}
