// IInventoryService.cs
using ColletteAPI.Models.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ColletteAPI.Services
{
    // Interface for inventory service
    public interface IInventoryService
    {
        Task SyncProductsToInventoryAsync();  // Sync all products to the inventory
        Task<IEnumerable<InventoryDto>> GetAllProductsAsync();  // Get all products with product details
    }
}
