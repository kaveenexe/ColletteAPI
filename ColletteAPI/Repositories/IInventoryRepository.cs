// IInventoryRepository.cs
using ColletteAPI.Models.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ColletteAPI.Repositories
{
    // Interface for inventory repository methods
    public interface IInventoryRepository
    {
        Task<Inventory> GetInventoryByProductIdAsync(string productId);  // Retrieve inventory by ProductId
        Task CreateInventoryAsync(Inventory inventory);  // Add a new inventory item
        Task UpdateInventoryAsync(Inventory inventory);  // Update an inventory item
        Task<IEnumerable<Inventory>> GetAllInventoriesAsync();  // Get all inventories

        Task<bool> DeleteInventoryItemAsync(string productId); // Add method to delete inventory item

    }
}
