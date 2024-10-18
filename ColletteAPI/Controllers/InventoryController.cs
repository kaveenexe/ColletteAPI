// InventoryController.cs
using ColletteAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ColletteAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        // Sync products to inventory (Add/Update)
        [HttpPost("sync")]
        public async Task<IActionResult> SyncProductsToInventory()
        {
            await _inventoryService.SyncProductsToInventoryAsync();
            return Ok("Products have been synced to the inventory.");
        }

        // Retrieves all products with their details (ProductId, Name, Quantity)
        [HttpGet("products")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _inventoryService.GetAllProductsAsync();
            return Ok(products);
        }

        // DELETE: api/inventory/{productId}
        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteInventoryItem(string productId)
        {
            // Attempt to delete the inventory item
            var success = await _inventoryService.DeleteInventoryItemAsync(productId);

            // If the deletion was blocked, return a 403 Forbidden response
            if (!success)
            {
                return Forbid("Deletion not allowed for Delivered or Cancelled orders.");
            }

            return NoContent(); // Return 204 No Content if successful
        }
    }
}
