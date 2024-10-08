/*
 * File: ProductRepository.cs
 * Description: Implements the IProductRepository interface, providing methods for product operations using MongoDB as the data store.
 */

using ColletteAPI.Models;
using MongoDB.Driver;
using MongoDB.Bson;

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

        // Retrieves all products for a specific vendor.
        public async Task<List<Product>> GetAllForVendorAsync(string vendorId)
        {
            return await _products.Find(p => p.VendorId == vendorId).ToListAsync();
        }

        // Retrieves a product by its ID.
        public async Task<Product> GetByIdAsync(string id)
        {
            return await _products.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        // Checks if a given unique product ID is already in use.
        public async Task<bool> IsUniqueProductIdUnique(string uniqueProductId)
        {
            return await _products.Find(p => p.UniqueProductId == uniqueProductId).FirstOrDefaultAsync() == null;
        }

        // Creates a new product.
        public async Task CreateAsync(Product product)
        {
            await _products.InsertOneAsync(product);
        }

        // Updates an existing product.
        public async Task UpdateAsync(string id, Product product)
        {
            await _products.ReplaceOneAsync(p => p.Id == id && p.VendorId == product.VendorId, product);
        }

        // Deletes a product.
        public async Task DeleteAsync(string id)
        {
            await _products.DeleteOneAsync(p => p.Id == id);
        }

        // Retrieves a product by its ID
        public async Task<Product> GetProductById(string productId)
        {
            return await _products.Find(p => p.UniqueProductId == productId).FirstOrDefaultAsync();
        }

        // Retrieves a product by its PId (possible alternative ID).
        public async Task<Product> GetProductByPId(string id)
        {
            return await _products.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        // Retrieves all products in the database.
        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _products.Find(_ => true).ToListAsync();
        }
    }
}