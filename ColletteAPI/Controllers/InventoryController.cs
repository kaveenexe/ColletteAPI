// InventoryController.cs
// Handles HTTP requests for inventory management.

using Microsoft.AspNetCore.Mvc;
using ColletteAPI.Services;
using ColletteAPI.Models.Dtos;

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

        // GET: api/inventory
        [HttpGet]
        public async Task<IActionResult> GetAllInventories()
        {
            var inventories = await _inventoryService.GetAllInventoriesAsync();
            return Ok(inventories);
        }

        // GET: api/inventory/{productId}
        [HttpGet("{productId}")]
        public async Task<IActionResult> GetInventoryByProductId(string productId)
        {
            var inventory = await _inventoryService.GetInventoryByProductIdAsync(productId);
            if (inventory == null)
            {
                return NotFound();
            }
            return Ok(inventory);
        }
    }
}
