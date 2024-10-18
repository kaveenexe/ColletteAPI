// InventoryService.cs
using ColletteAPI.Models.Domain;
using ColletteAPI.Models.Dtos;
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
        private readonly IOrderRepository _orderRepository; // Assuming you have an OrderRepository to handle orders
        private readonly INotificationRepository _notificationRepository;


        public InventoryService(IInventoryRepository inventoryRepository, IProductRepository productRepository, IOrderRepository orderRepository, INotificationRepository notificationRepository)
        {
            _inventoryRepository = inventoryRepository;
            _productRepository = productRepository;
            _orderRepository = orderRepository; // Injecting OrderRepository
            _notificationRepository = notificationRepository;

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
                        VendorId = product.VendorId,
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

        // Retrieves all products with product details (ProductId, Name, StockQuantity)
        public async Task<IEnumerable<InventoryDto>> GetAllProductsAsync()
        {
            // Sync products to inventory before fetching them
            await SyncProductsToInventoryAsync();

            var inventories = await _inventoryRepository.GetAllInventoriesAsync();
            var productDetails = new List<InventoryDto>();

            foreach (var inventory in inventories)
            {
                // Fetch the product details using the ProductId from ProductRepository
                var product = await _productRepository.GetProductById(inventory.ProductId);

                // Check if the stock quantity is below 5 and notify the vendor
                if (product.StockQuantity < 5)
                {
                    var lowStockMessage = $"Your product {product.Name} has low stock. Only {product.StockQuantity} items left.";

                    // Create a notification for the vendor about low stock
                    var notification = new Notification
                    {
                        Message = lowStockMessage,
                        IsVisibleToCSR = false,
                        IsVisibleToAdmin = false,
                        IsVisibleToVendor = true,
                        IsVisibleToCustomer = false,
                        IsResolved = false,
                        VendorId = product.VendorId  // Notify the vendor of the product
                    };

                    await _notificationRepository.AddNotification(notification);
                }

                // Add the product details along with the inventory stock quantity
                productDetails.Add(new InventoryDto
                {
                    Id = inventory.Id,
                    ProductId = product?.UniqueProductId,
                    ProductName = product?.Name,
                    VendorId = product.VendorId,
                    StockQuantity = product.StockQuantity,  // Quantity from Inventory table
                    Category = product?.Category.ToString()  // Assuming Category is an enum or string
                });
            }

            return productDetails;
        }

        // Delete inventory item with order status check
        public async Task<bool> DeleteInventoryItemAsync(string productId)
        {
            // Call the repository method to delete the item, with order status check
            return await _inventoryRepository.DeleteInventoryItemAsync(productId);
        }
    }
}
