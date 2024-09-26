using Microsoft.AspNetCore.Mvc;
using ColletteAPI.Models;
using ColletteAPI.Repositories;
using System.Threading.Tasks;

namespace ColletteAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;

        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<Cart>> GetCart(string userId)
        {
            var cart = await _cartRepository.GetCartAsync(userId);
            return Ok(cart);
        }

        [HttpPost("{userId}/items")]
        public async Task<ActionResult> AddToCart(string userId, [FromBody] CartItem item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _cartRepository.AddToCartAsync(userId, item);
            return Ok();
        }

        [HttpDelete("{userId}/items/{productId}")]
        public async Task<ActionResult> RemoveFromCart(string userId, string productId)
        {
            await _cartRepository.RemoveFromCartAsync(userId, productId);
            return Ok();
        }

        [HttpPut("{userId}/items/{productId}")]
        public async Task<ActionResult> UpdateCartItemQuantity(string userId, string productId, [FromBody] int quantity)
        {
            if (quantity < 1)
            {
                return BadRequest("Quantity must be greater than zero.");
            }

            await _cartRepository.UpdateCartItemQuantityAsync(userId, productId, quantity);
            return Ok();
        }

        [HttpDelete("{userId}")]
        public async Task<ActionResult> ClearCart(string userId)
        {
            await _cartRepository.ClearCartAsync(userId);
            return Ok();
        }
    }
}