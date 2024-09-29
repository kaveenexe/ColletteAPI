using ColletteAPI.Models.Domain;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ColletteAPI.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IMongoCollection<Order> _orders;

        public OrderRepository(IMongoClient client, IConfiguration configuration)
        {
            var database = client.GetDatabase(configuration.GetSection("MongoDB:DatabaseName").Value);
            _orders = database.GetCollection<Order>("Orders");
        }

        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            return await _orders.Find(order => true).ToListAsync();
        }

        public async Task<Order> GetOrderById(string id)
        {
            return await _orders.Find(order => order.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Order> CreateOrder(Order order)
        {
            await _orders.InsertOneAsync(order);
            return order;
        }

        public async Task<bool> UpdateOrderStatus(string id, OrderStatus status)
        {
            var updateDefinition = Builders<Order>.Update.Set(o => o.Status, status);
            var result = await _orders.UpdateOneAsync(o => o.Id == id, updateDefinition);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> CancelOrder(string id, string adminNote)
        {
            var order = await GetOrderById(id);
            if (order == null)
            {
                return false; 
            }

            var cancellation = new OrderCancellation
            {
                Id = ObjectId.GenerateNewId().ToString(), 
                OrderId = id,
                CancellationApproved = false, 
                AdminNote = adminNote,
                CancellationDate = DateTime.UtcNow 
            };

            var updateDefinition = Builders<Order>.Update
                .Set(o => o.OrderCancellation, cancellation)
                .Set(o => o.Status, OrderStatus.Cancelled); 

            var result = await _orders.UpdateOneAsync(o => o.Id == id, updateDefinition);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> OrderExists(string orderId)
        {
            var order = await _orders.Find(o => o.OrderId == orderId).FirstOrDefaultAsync();
            return order != null;
        }

        public async Task<bool> MarkOrderAsDelivered(string id)
        {
            var updateDefinition = Builders<Order>.Update
                .Set(o => o.Status, OrderStatus.Delivered);

            var result = await _orders.UpdateOneAsync(o => o.Id == id, updateDefinition);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteOrder(string id)
        {
            var result = await _orders.DeleteOneAsync(o => o.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<string> GetOrderStatus(string id)
        {
            var order = await GetOrderById(id);
            return order?.Status.ToString() ?? "Order not found";
        }
    }
}
