﻿using ColletteAPI.Models;
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

        public async Task<List<Product>> GetAllForVendorAsync(string vendorId)
        {
            return await _products.Find(p => p.VendorId == vendorId).ToListAsync();
        }

        public async Task<Product> GetByIdAsync(string id)
        {
            return await _products.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> IsUniqueProductIdUnique(string uniqueProductId)
        {
            return await _products.Find(p => p.UniqueProductId == uniqueProductId).FirstOrDefaultAsync() == null;
        }

        public async Task CreateAsync(Product product)
        {
            await _products.InsertOneAsync(product);
        }

        public async Task UpdateAsync(string id, Product product)
        {
            await _products.ReplaceOneAsync(p => p.Id == id && p.VendorId == product.VendorId, product);
        }

        public async Task DeleteAsync(string id)
        {
            await _products.DeleteOneAsync(p => p.Id == id);
        }

        public async Task<Product> GetProductById(string productId)
        {
            return await _products.Find(p => p.UniqueProductId == productId).FirstOrDefaultAsync();
        }

        public async Task<Product> GetProductByPId(string id)
        {
            return await _products.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _products.Find(_ => true).ToListAsync();
        }
    }
}