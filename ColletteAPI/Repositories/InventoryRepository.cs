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
        private readonly IOrderRepository _orderRepository; // Dependency on OrderRepository

        public InventoryRepository(IMongoDatabase database, IOrderRepository orderRepository)
        {
            _inventoryCollection = database.GetCollection<Inventory>("Inventories");
            _orderRepository = orderRepository; // Inject OrderRepository

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



        // Delete inventory item if conditions are met based on order status
        public async Task<bool> DeleteInventoryItemAsync(string productId)
        {
            // Fetch all orders related to the product
            var orders = await _orderRepository.GetOrdersByProductId(productId);

            // Check the order status for all related orders
            foreach (var order in orders)
            {
                // If any order is in a non-deletable state, block the deletion
                if (order.Status == OrderStatus.Delivered || order.Status == OrderStatus.Cancelled || order.Status == OrderStatus.Pending)
                {
                    return false; // Block deletion
                }
            }

            // If no conflicting statuses (Delivered/Cancelled), proceed to delete the inventory item
            var result = await _inventoryCollection.DeleteOneAsync(i => i.ProductId == productId);

            // Return true if the deletion was successful
            return result.DeletedCount > 0;
        }
    }
}
