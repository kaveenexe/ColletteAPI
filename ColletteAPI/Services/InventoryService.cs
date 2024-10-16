// InventoryService.cs
using ColletteAPI.Models.Domain;
using ColletteAPI.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ColletteAPI.Services
{
    // Implementation of inventory service
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IProductRepository _productRepository;  // Using ProductRepository

        public InventoryService(IInventoryRepository inventoryRepository, IProductRepository productRepository)
        {
            _inventoryRepository = inventoryRepository;
            _productRepository = productRepository;
        }

        // Syncs all products to inventories
        public async Task SyncProductsToInventoryAsync()
        {
            // Retrieve all products from the ProductRepository
            var products = await _productRepository.GetAllProductsAsync();

            foreach (var product in products)
            {
                // Check if the product already exists in the inventory
                var inventoryItem = await _inventoryRepository.GetInventoryByProductIdAsync(product.UniqueProductId);

                if (inventoryItem == null)
                {
                    // If not in inventory, create a new inventory entry for the product
                    var newInventory = new Inventory
                    {
                        ProductId = product.UniqueProductId,
                        StockQuantity = product.StockQuantity // Initialize quantity based on the product
                    };
                    await _inventoryRepository.CreateInventoryAsync(newInventory);
                }
                else
                {
                    // If the product exists in inventory, update the quantity
                    inventoryItem.StockQuantity = product.StockQuantity;
                    await _inventoryRepository.UpdateInventoryAsync(inventoryItem);
                }
            }
        }
    }
}
