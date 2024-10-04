//IInventoryService.cs
using ColletteAPI.Models.Dtos;

namespace ColletteAPI.Services
{
    public interface IInventoryService
    {
        Task<IEnumerable<InventoryDto>> GetAllInventoriesAsync();
        Task<InventoryDto> GetInventoryByIdAsync(string inventoryId);
        Task<InventoryDto> GetInventoryByProductIdAsync(string productId);
        Task AddInventoryAsync(CreateInventoryDto inventoryDto);
        Task UpdateInventoryAsync(string productId, UpdateInventoryDto inventoryDto);
        Task RemoveInventoryAsync(string inventoryId, string orderStatus); // Include order status for condition
        Task UpdateInventoryForProductCreation(string productId, int quantity); // New for vendor add
        Task UpdateInventoryForProductPurchase(string productId, int quantity); // New for purchase decrement
    }
}
