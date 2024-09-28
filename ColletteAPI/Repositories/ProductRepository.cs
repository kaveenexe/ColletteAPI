using System.Collections.Generic;
using System.Threading.Tasks;
using ColletteAPI.Data;
using ColletteAPI.Models;
using MongoDB.Driver;
namespace ColletteAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMongoCollection<Product> _products;

        public ProductRepository(IMongoClient client, IConfiguration configuration)
        {
            var database = client.GetDatabase(configuration.GetSection("MongoDB:DatabaseName").Value);
            _products = database.GetCollection<Product>("Products");
        }


        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _products.Find(product => true).ToListAsync();
        }

        public async Task<Product> GetProductById(string id)
        {
            return await _products.Find(product => product.Id == id).FirstOrDefaultAsync();
        }

        public async Task AddProduct(Product product)
        {
            await _products.InsertOneAsync(product);
        }

        public async Task UpdateProduct(string id, Product product)
        {
            await _products.ReplaceOneAsync(prod => prod.Id == id, product);
        }

        public async Task DeleteProduct(string id)
        {
            await _products.DeleteOneAsync(prod => prod.Id == id);
        }
    }
}
