// IInventoryRepository.cs
// Interface for Inventory repository

using ColletteAPI.Models.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ColletteAPI.Repositories
{
    using ColletteAPI.Models.Domain;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IInventoryRepository
    {
        Task<IEnumerable<Inventory>> GetAllInventoriesAsync();
        Task<Inventory> GetInventoryByProductIdAsync(string productId);
        Task<Inventory> AddInventoryAsync(Inventory inventory);
        Task<bool> UpdateInventoryAsync(Inventory inventory);
        Task<bool> DeleteInventoryAsync(string id);
    }
}
