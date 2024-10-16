// IInventoryService.cs
using System.Threading.Tasks;

namespace ColletteAPI.Services
{
    // Interface for inventory service
    public interface IInventoryService
    {
        Task SyncProductsToInventoryAsync();  // Sync all products to the inventory
    }
}
