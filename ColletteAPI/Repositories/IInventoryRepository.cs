// IInventoryRepository.cs
using ColletteAPI.Models.Domain;
using System.Threading.Tasks;

namespace ColletteAPI.Repositories
{
    // Interface for inventory repository methods
    public interface IInventoryRepository
    {
        Task<Inventory> GetInventoryByProductIdAsync(string productId);  // Retrieve inventory by ProductId
        Task CreateInventoryAsync(Inventory inventory);  // Add a new inventory item
        Task UpdateInventoryAsync(Inventory inventory);  // Update an inventory item
    }
}
