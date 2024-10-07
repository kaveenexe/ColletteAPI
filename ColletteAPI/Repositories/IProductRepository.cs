using System.Collections.Generic;
using System.Threading.Tasks;
using ColletteAPI.Models;

namespace ColletteAPI.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllForVendorAsync(string vendorId);
        Task<Product> GetByIdAsync(string id);
        Task<bool> IsUniqueProductIdUnique(string uniqueProductId);
        Task CreateAsync(Product product);
        Task UpdateAsync(string id, Product product);
        Task DeleteAsync(string id);
        Task<Product> GetProductById(string productId);

        Task<Product> GetProductByPId(string id);

        Task<List<Product>> GetAllProductsAsync();
    }
}