using ColletteAPI.Models;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace ColletteAPI.Repositories
{
   
    public class CartRepository : ICartRepository
    {
        private readonly IMongoCollection<Cart> _carts;

        public CartRepository(IMongoClient client, IConfiguration configuration)
        {
            var database = client.GetDatabase(configuration.GetSection("MongoDB:DatabaseName").Value);
            _carts = database.GetCollection<Cart>("Carts");
        }

        public async Task<Cart> GetCartAsync(string userId)
        {
            return await _carts.Find(c => c.UserId == userId).FirstOrDefaultAsync()
                ?? new Cart { UserId = userId };
        }

        public async Task AddToCartAsync(string userId, CartItem item)
        {
            var cart = await GetCartAsync(userId);
            var existingItem = cart.Items.Find(i => i.ProductId == item.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                cart.Items.Add(item);
            }

            cart.TotalPrice = cart.Items.Sum(i => i.Price * i.Quantity);

            await _carts.ReplaceOneAsync(c => c.UserId == userId, cart, new ReplaceOptions { IsUpsert = true });
        }

        public async Task RemoveFromCartAsync(string userId, string productId)
        {
            var cart = await GetCartAsync(userId);
            cart.Items.RemoveAll(i => i.ProductId == productId);
            cart.TotalPrice = cart.Items.Sum(i => i.Price * i.Quantity);

            await _carts.ReplaceOneAsync(c => c.UserId == userId, cart);
        }

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

        public async Task ClearCartAsync(string userId)
        {
            await _carts.DeleteOneAsync(c => c.UserId == userId);
        }
    }
}