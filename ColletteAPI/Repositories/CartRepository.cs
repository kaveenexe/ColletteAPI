/*
 * File: CartRepository.cs
 * Description: Implements the ICartRepository interface, providing methods for cart operations using MongoDB as the data store.
 */

using ColletteAPI.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace ColletteAPI.Repositories
{
    // Interface defining the contract for cart repository operations.
    public class CartRepository : ICartRepository
    {
        private readonly IMongoCollection<Cart> _carts;

        public CartRepository(IMongoClient client, IConfiguration configuration)
        {
            var database = client.GetDatabase(configuration.GetSection("MongoDB:DatabaseName").Value);
            _carts = database.GetCollection<Cart>("Carts");
        }

        // Retrieves the cart for a specific user.
        public async Task<Cart> GetCartAsync(string userId)
        {
            return await _carts.Find(c => c.UserId == userId).FirstOrDefaultAsync()
                ?? new Cart { UserId = userId };
        }

        // Retrieves the cart for a specific user and cart id.
        public async Task<Cart> GetCartByUserIdAndCartId(string userId, string cartId)
        {
            return await _carts.Find(c => c.UserId == userId && c.Id == cartId).FirstOrDefaultAsync()
                ?? new Cart { UserId = userId };
        }

        // Adds an item to the user's cart.
        public async Task AddToCartAsync(string userId, CartItem newItem)
        {
            var filter = Builders<Cart>.Filter.Eq(c => c.UserId, userId);
            var cart = await _carts.Find(filter).FirstOrDefaultAsync();

            if (cart == null)
            {
                // Create a new cart if it doesn't exist
                cart = new Cart
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    UserId = userId,
                    Items = new List<CartItem> { newItem },
                    TotalPrice = newItem.Price * newItem.Quantity
                };
                await _carts.InsertOneAsync(cart);
            }
            else
            {
                // Update existing cart
                var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == newItem.ProductId);
                if (existingItem != null)
                {
                    // Update quantity of existing item
                    existingItem.Quantity += newItem.Quantity;
                }
                else
                {
                    // Add new item to the cart
                    cart.Items.Add(newItem);
                }

                // Recalculate total price
                cart.TotalPrice = cart.Items.Sum(i => i.Price * i.Quantity);

                // Update the cart in the database
                var update = Builders<Cart>.Update
                    .Set(c => c.Items, cart.Items)
                    .Set(c => c.TotalPrice, cart.TotalPrice);

                await _carts.UpdateOneAsync(filter, update);
            }
        }

        // Removes an item from the user's cart.
        public async Task RemoveFromCartAsync(string userId, string productId)
        {
            var cart = await GetCartAsync(userId);
            cart.Items.RemoveAll(i => i.ProductId == productId);
            cart.TotalPrice = cart.Items.Sum(i => i.Price * i.Quantity);

            await _carts.ReplaceOneAsync(c => c.UserId == userId, cart);
        }

        // Updates the quantity of an item in the user's cart.
        public async Task UpdateCartItemQuantityAsync(string userId, string productId, int quantity)
        {
            var cart = await GetCartAsync(userId);
            var item = cart.Items.Find(i => i.ProductId == productId);

            if (item != null)
            {
                item.Quantity = quantity;
                cart.TotalPrice = cart.Items.Sum(i => i.Price * i.Quantity);

                await _carts.ReplaceOneAsync(c => c.UserId == userId, cart);
            }
        }

        // Clears all items from the user's cart.
        public async Task ClearCartAsync(string userId)
        {
            await _carts.DeleteOneAsync(c => c.UserId == userId);
        }
    }
}