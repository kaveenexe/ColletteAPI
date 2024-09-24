using System.Collections.Generic;
using System.Threading.Tasks;
using ColletteAPI.Models;
namespace ColletteAPI.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product> GetProductById(string id);
        Task AddProduct(Product product);
        Task UpdateProduct(string id, Product product);
        Task DeleteProduct(string id);
    }
}
