using ColletteAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ColletteAPI.Repositories
{

    public interface ICartRepository
    {
        Task<Cart> GetCartAsync(string userId);
        Task AddToCartAsync(string userId, CartItem item);
        Task RemoveFromCartAsync(string userId, string productId);
        Task UpdateCartItemQuantityAsync(string userId, string productId, int quantity);
        Task ClearCartAsync(string userId);
    }
}