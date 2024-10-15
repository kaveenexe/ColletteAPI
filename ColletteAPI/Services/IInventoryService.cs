// IInventoryService.cs
// Interface for the Inventory service layer.

using ColletteAPI.Models.Dtos;

namespace ColletteAPI.Services
{
    public interface IInventoryService
    {
        Task<IEnumerable<InventoryDto>> GetAllInventoriesAsync();
        Task<InventoryDto> GetInventoryByProductIdAsync(string productId);
    }
}
