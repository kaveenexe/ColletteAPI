
using ColletteAPI.Models.Dtos;

namespace ColletteAPI.Services
{
    public interface IInventoryService
    {
        Task<IEnumerable<InventoryDto>> GetAllInventoriesAsync();
        Task<InventoryDto> GetInventoryByIdAsync(int inventoryId);
        Task<InventoryDto> GetInventoryByProductIdAsync(int productId);
        Task AddInventoryAsync(CreateInventoryDto inventoryDto);
        Task UpdateInventoryAsync(int productId, UpdateInventoryDto inventoryDto);
        Task RemoveInventoryAsync(int inventoryId, string orderStatus); // Include order status for condition
        Task UpdateInventoryForProductCreation(int productId, int quantity); // New for vendor add
        Task UpdateInventoryForProductPurchase(int productId, int quantity); // New for purchase decrement
    }
}
