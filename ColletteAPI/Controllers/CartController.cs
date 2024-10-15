/*
 * File: CartController.cs
 * Description: Handles HTTP requests related to shopping cart operations, including retrieving, adding, updating, and removing items from a user's cart.
 */

using Microsoft.AspNetCore.Mvc;
using ColletteAPI.Models;
using ColletteAPI.Repositories;
using System.Threading.Tasks;

namespace ColletteAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    // Constructor for CartController.
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;

        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        // Retrieves the cart for a specific user.
        [HttpGet("{userId}")]
        public async Task<ActionResult<Cart>> GetCart(string userId)
        {
            var cart = await _cartRepository.GetCartAsync(userId);
            return Ok(cart);
        }

        // Retrieves the cart for a specific user and cart id.
        [HttpGet("{userId}/{cartId}")]
        public async Task<ActionResult<Cart>> GetCart(string userId, string cartId)
        {
            var cart = await _cartRepository.GetCartByUserIdAndCartId(userId, cartId);
            if (cart == null)
            {
                return NotFound("Cart not found for the provided user and cart ID.");
            }
            return Ok(cart);
        }

        // Adds an item to the user's cart.
        [HttpPost("{userId}/items")]
        public async Task<IActionResult> AddToCart(string userId, [FromBody] CartItem item)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID is required");
            }

            if (item.Quantity <= 0)
            {
                return BadRequest("Quantity must be greater than 0");
            }

            await _cartRepository.AddToCartAsync(userId, item);
            return Ok();
        }

        // Removes an item from the user's cart.
        [HttpDelete("{userId}/items/{productId}")]
        public async Task<ActionResult> RemoveFromCart(string userId, string productId)
        {
            await _cartRepository.RemoveFromCartAsync(userId, productId);
            return Ok();
        }

        // DTO for updating cart item quantity.
        public class UpdateQuantityRequest
        {
            public int Quantity { get; set; }
        }

        // Updates the quantity of an item in the user's cart.
        [HttpPut("{userId}/items/{productId}")]
        public async Task<ActionResult> UpdateCartItemQuantity(string userId, string productId, [FromBody] UpdateQuantityRequest request)
        {
            if (request == null || request.Quantity < 1)
            {
                return BadRequest("Quantity must be greater than zero.");
            }
            await _cartRepository.UpdateCartItemQuantityAsync(userId, productId, request.Quantity);
            return Ok();
        }

        // Clears all items from the user's cart.
        [HttpDelete("{userId}")]
        public async Task<ActionResult> ClearCart(string userId)
        {
            await _cartRepository.ClearCartAsync(userId);
            return Ok();
        }
    }
}