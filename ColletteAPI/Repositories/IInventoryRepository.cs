//IInventoryRepository.cs

using ColletteAPI.Models.Domain;

namespace ColletteAPI.Repositories
{
    public interface IInventoryRepository
    {
        Task<IEnumerable<Inventory>> GetAllInventoriesAsync();
        Task<Inventory> GetInventoryByIdAsync(string inventoryId);
        Task<Inventory> GetInventoryByProductIdAsync(string productId);
        Task AddInventoryAsync(Inventory inventory);
        Task UpdateInventoryAsync(Inventory inventory);
        Task RemoveInventoryAsync(string inventoryId, string orderStatus); // Updated for conditions
        Task<bool> InventoryExistsAsync(string productId);
    }
}
