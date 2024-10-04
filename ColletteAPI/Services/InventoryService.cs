//InventoryService.cs

using ColletteAPI.Models.Domain;
using ColletteAPI.Models.Dtos;
using ColletteAPI.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ColletteAPI.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _inventoryRepository;

        public InventoryService(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        public async Task<IEnumerable<InventoryDto>> GetAllInventoriesAsync()
        {
            var inventories = await _inventoryRepository.GetAllInventoriesAsync();
            return inventories.Select(i => new InventoryDto
            {
                ProductId = i.ProductId,
                ProductName = i.Product.Name,
                Quantity = i.Quantity,
                IsLowStockAlert = i.IsLowStockAlert
            });
        }

        public async Task<InventoryDto> GetInventoryByIdAsync(string inventoryId)
        {
            var inventory = await _inventoryRepository.GetInventoryByIdAsync(inventoryId);
            if (inventory == null) return null;

            return new InventoryDto
            {
                ProductId = inventory.ProductId,
                ProductName = inventory.Product.Name,
                Quantity = inventory.Quantity,
                IsLowStockAlert = inventory.IsLowStockAlert
            };
        }

        public async Task<InventoryDto> GetInventoryByProductIdAsync(string productId)
        {
            var inventory = await _inventoryRepository.GetInventoryByProductIdAsync(productId);
            if (inventory == null) return null;

            return new InventoryDto
            {
                ProductId = inventory.ProductId,
                ProductName = inventory.Product.Name,
                Quantity = inventory.Quantity,
                IsLowStockAlert = inventory.IsLowStockAlert
            };
        }

        public async Task AddInventoryAsync(CreateInventoryDto inventoryDto)
        {
            var inventory = new Inventory
            {
                ProductId = inventoryDto.ProductId,
                Quantity = inventoryDto.Quantity,
                IsLowStockAlert = inventoryDto.Quantity <= 5 // Alert for low stock
            };

            await _inventoryRepository.AddInventoryAsync(inventory);
        }

        public async Task UpdateInventoryAsync(string productId, UpdateInventoryDto inventoryDto)
        {
            var inventory = await _inventoryRepository.GetInventoryByProductIdAsync(productId);
            if (inventory == null) return;

            inventory.Quantity = inventoryDto.Quantity;
            inventory.IsLowStockAlert = inventoryDto.Quantity <= 5;

            await _inventoryRepository.UpdateInventoryAsync(inventory);
        }

        public async Task RemoveInventoryAsync(string inventoryId, string orderStatus)
        {
            await _inventoryRepository.RemoveInventoryAsync(inventoryId, orderStatus);
        }

        // Automatic stock increment when vendor adds a product
        public async Task UpdateInventoryForProductCreation(string productId, int quantity)
        {
            var inventory = await _inventoryRepository.GetInventoryByProductIdAsync(productId);
            if (inventory != null)
            {
                inventory.AddStock(quantity);
                await _inventoryRepository.UpdateInventoryAsync(inventory);
            }
        }

        // Automatic stock decrement when a user buys a product
        public async Task UpdateInventoryForProductPurchase(string productId, int quantity)
        {
            var inventory = await _inventoryRepository.GetInventoryByProductIdAsync(productId);
            if (inventory != null)
            {
                inventory.RemoveStock(quantity);
                await _inventoryRepository.UpdateInventoryAsync(inventory);
            }
        }
    }
}
