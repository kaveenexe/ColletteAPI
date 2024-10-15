// InventoryService.cs
// Implements business logic for inventory management.

using ColletteAPI.Repositories;
using ColletteAPI.Models.Domain;
using ColletteAPI.Models.Dtos;

namespace ColletteAPI.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IProductRepository _productRepository; // Assuming ProductRepository is injected

        public InventoryService(IInventoryRepository inventoryRepository, IProductRepository productRepository)
        {
            _inventoryRepository = inventoryRepository;
            _productRepository = productRepository;
        }

        // Fetch all inventories and include product names
        public async Task<IEnumerable<InventoryDto>> GetAllInventoriesAsync()
        {
            var inventories = await _inventoryRepository.GetAllInventoriesAsync();
            var inventoryDtos = new List<InventoryDto>();

            foreach (var inventory in inventories)
            {
                var product = await _productRepository.GetByIdAsync(inventory.ProductId); // Assumed method
                if (product != null)
                {
                    inventoryDtos.Add(new InventoryDto
                    {
                        ProductName = product.Name,
                        Quantity = inventory.Quantity
                    });
                }
            }

            return inventoryDtos;
        }

        // Fetch inventory by product ID and include product name
        public async Task<InventoryDto> GetInventoryByProductIdAsync(string productId)
        {
            var inventory = await _inventoryRepository.GetInventoryByProductIdAsync(productId);
            if (inventory != null)
            {
                var product = await _productRepository.GetByIdAsync(productId); // Assumed method
                if (product != null)
                {
                    return new InventoryDto
                    {
                        ProductName = product.Name,
                        Quantity = inventory.Quantity
                    };
                }
            }
            return null;
        }
    }
}
