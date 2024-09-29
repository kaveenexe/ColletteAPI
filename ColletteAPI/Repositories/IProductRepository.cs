using System.Collections.Generic;
using System.Threading.Tasks;
using ColletteAPI.Models;

namespace ColletteAPI.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllByVendorIdAsync(string vendorId);
        Task<Product> GetByIdAndVendorIdAsync(string id, string vendorId);
        Task<bool> IsUniqueProductIdUnique(string uniqueProductId);
        Task CreateAsync(Product product);
        Task UpdateAsync(string id, Product product);
        Task DeleteAsync(string id);
    }
}