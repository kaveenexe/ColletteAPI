
using ColletteAPI.Models.Domain;

namespace ColletteAPI.Repositories
{
    public interface IInventoryRepository
    {
        Task<IEnumerable<Inventory>> GetAllInventoriesAsync();
        Task<Inventory> GetInventoryByIdAsync(int inventoryId);
        Task<Inventory> GetInventoryByProductIdAsync(int productId);
        Task AddInventoryAsync(Inventory inventory);
        Task UpdateInventoryAsync(Inventory inventory);
        Task RemoveInventoryAsync(int inventoryId, string orderStatus); // Updated for conditions
        Task<bool> InventoryExistsAsync(int productId);
    }
}
