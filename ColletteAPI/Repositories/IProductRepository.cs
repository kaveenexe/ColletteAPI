/*
 * File: IProductRepository.cs
 * Description: Defines the interface for product repository operations, specifying methods for managing and retrieving product information.
 */

using System.Collections.Generic;
using System.Threading.Tasks;
using ColletteAPI.Models;

namespace ColletteAPI.Repositories
{
    // Interface defining the contract for product repository operations.
    public interface IProductRepository
    {
        // Retrieves all products for a specific vendor.
        Task<List<Product>> GetAllForVendorAsync(string vendorId);

        // Retrieves a product by its ID.
        Task<Product> GetByIdAsync(string id);

        // Checks if a given unique product ID is already in use.
        Task<bool> IsUniqueProductIdUnique(string uniqueProductId);

        // Creates a new product.
        Task CreateAsync(Product product);

        // Updates an existing product.
        Task UpdateAsync(string id, Product product);

        // Deletes a product.
        Task DeleteAsync(string id);

        // Retrieves a product by its ID.
        Task<Product> GetProductById(string productId);

        // Retrieves a product by its PId (possible alternative ID).
        Task<Product> GetProductByPId(string id);

        // Retrieves all products in the database.
        Task<List<Product>> GetAllProductsAsync();
    }
}