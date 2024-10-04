
using ColletteAPI.Models.Dtos;
using ColletteAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ColletteAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllInventories()
        {
            var inventories = await _inventoryService.GetAllInventoriesAsync();
            return Ok(inventories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInventoryById(int id)
        {
            var inventory = await _inventoryService.GetInventoryByIdAsync(id);
            if (inventory == null) return NotFound();

            return Ok(inventory);
        }

        [HttpPost]
        public async Task<IActionResult> AddInventory([FromBody] CreateInventoryDto inventoryDto)
        {
            await _inventoryService.AddInventoryAsync(inventoryDto);
            return CreatedAtAction(nameof(GetInventoryById), new { id = inventoryDto.ProductId }, inventoryDto);
        }

        [HttpPut("{productId}")]
        public async Task<IActionResult> UpdateInventory(int productId, [FromBody] UpdateInventoryDto inventoryDto)
        {
            await _inventoryService.UpdateInventoryAsync(productId, inventoryDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveInventory(int id, [FromQuery] string orderStatus)
        {
            await _inventoryService.RemoveInventoryAsync(id, orderStatus);
            return NoContent();
        }

        // Vendor adds product -> update inventory
        [HttpPost("vendor/{productId}/add")]
        public async Task<IActionResult> VendorAddProduct(int productId, [FromQuery] int quantity)
        {
            await _inventoryService.UpdateInventoryForProductCreation(productId, quantity);
            return NoContent();
        }

        // User buys product -> update inventory
        [HttpPost("user/{productId}/purchase")]
        public async Task<IActionResult> UserPurchaseProduct(int productId, [FromQuery] int quantity)
        {
            await _inventoryService.UpdateInventoryForProductPurchase(productId, quantity);
            return NoContent();
        }
    }
}
