/*
 * File: ICartRepository.cs
 * Description: Defines the interface for cart repository operations, specifying methods for managing cart items and retrieving cart information.
 */

using ColletteAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ColletteAPI.Repositories
{

    public interface ICartRepository
    {
        // Retrieves the cart for a specific user.
        Task<Cart> GetCartAsync(string userId);

        // Adds an item to the user's cart.
        Task AddToCartAsync(string userId, CartItem item);

        // Removes an item from the user's cart.
        Task RemoveFromCartAsync(string userId, string productId);

        // Updates the quantity of an item in the user's cart.
        Task UpdateCartItemQuantityAsync(string userId, string productId, int quantity);

        // Clears all items from the user's cart.
        Task ClearCartAsync(string userId);

        // Retrieves the cart for a specific user and cart id.
        Task<Cart> GetCartByUserIdAndCartId(string userId, string cartId);
    }
}