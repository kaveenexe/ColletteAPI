using ColletteAPI.Models.Domain;
using ColletteAPI.Models.Dtos;
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

        // Get all orders
        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            return await _orders.Find(order => true).ToListAsync();
        }

        // Get order by ID
        public async Task<Order> GetOrderById(string id)
        {
            return await _orders.Find(order => order.Id == id).FirstOrDefaultAsync();
        }

        // Get order by customerId
        public async Task<IEnumerable<Order>> GetOrdersByCustomerId(string customerId)
        {
            return await _orders.Find(o => o.CustomerId == customerId).ToListAsync();
        }

        // Get order by customerId and orderId
        public async Task<Order> GetOrderByCustomerIdAndOrderId(string customerId, string orderId)
        {
            return await _orders.Find(order => order.CustomerId == customerId && order.OrderId == orderId).FirstOrDefaultAsync();
        }

        // Create order by customer
        public async Task<Order> CreateOrderByCustomer(Order order)
        {
            await _orders.InsertOneAsync(order);
            return order;
        }

        // Create order by admin
        public async Task<Order> CreateOrderByAdmin(Order order)
        {
            await _orders.InsertOneAsync(order);
            return order;
        }

        // Check if the order is exists
        public async Task<bool> OrderExists(string orderId)
        {
            var order = await _orders.Find(o => o.OrderId == orderId).FirstOrDefaultAsync();
            return order != null;
        }

        // Update order
        public async Task<bool> UpdateOrder(Order order)
        {
            var updateDefinition = Builders<Order>.Update
                .Set(o => o.Status, order.Status)
                .Set(o => o.OrderCancellation, order.OrderCancellation)
                .Set(o => o.OrderItems, order.OrderItems)
                .Set(o => o.TotalAmount, order.TotalAmount);

            var result = await _orders.UpdateOneAsync(
                o => o.Id == order.Id,
                updateDefinition
            );

            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        // Delete order
        public async Task<bool> DeleteOrder(string id)
        {
            var result = await _orders.DeleteOneAsync(o => o.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        // Update order status
        public async Task<bool> UpdateOrderStatus(string id, OrderStatus status)
        {
            var updateDefinition = Builders<Order>.Update.Set(o => o.Status, status);
            var result = await _orders.UpdateOneAsync(o => o.Id == id, updateDefinition);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        // Request Order Cancellation
        public async Task<bool> RequestOrderCancellation(OrderCancellationDto cancellationDto)
        {
            var order = await GetOrderById(cancellationDto.Id);

            if (order == null || order.Status == OrderStatus.Cancelled)
            {
                return false;
            }

            var cancellation = new OrderCancellation
            {
                Id = ObjectId.GenerateNewId().ToString(),
                OrderId = order.OrderId,
                CancellationApproved = false,
                CancellationDate = DateTime.UtcNow,
                CancelRequestStatus = CancelRequestStatus.Pending
            };

            var updateDefinition = Builders<Order>.Update
                .Set(o => o.OrderCancellation, cancellation);

            var result = await _orders.UpdateOneAsync(o => o.Id == order.Id, updateDefinition);

            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        // Cancel Order
        public async Task<bool> CancelOrder(string id)
        {
            var order = await GetOrderById(id);
            if (order == null || order.Status == OrderStatus.Delivered || order.Status == OrderStatus.PartiallyDelivered)
            {
                return false;
            }

            var cancellation = new OrderCancellation
            {
                Id = ObjectId.GenerateNewId().ToString(),
                OrderId = id,
                CancellationApproved = true,
                CancellationDate = DateTime.UtcNow,
                CancelRequestStatus= CancelRequestStatus.Accepted
            };

            var updateDefinition = Builders<Order>.Update
                .Set(o => o.OrderCancellation, cancellation)
                .Set(o => o.Status, OrderStatus.Cancelled);

            var result = await _orders.UpdateOneAsync(o => o.Id == id, updateDefinition);

            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        // Get order status
        public async Task<string> GetOrderStatus(string id)
        {
            var order = await GetOrderById(id);
            return order?.Status.ToString() ?? "Order not found";
        }

        // Get order by status
        public async Task<List<Order>> GetOrdersByStatus(OrderStatus status)
        {
            return await _orders.Find(order => order.Status == status).ToListAsync();
        }

        // Get orders by cancellation request status
        public async Task<List<Order>> GetOrdersByCancelRequestStatus(CancelRequestStatus cancelRequestStatus)
        {
            return await _orders
                .Find(order => order.OrderCancellation != null && order.OrderCancellation.CancelRequestStatus == cancelRequestStatus)
                .ToListAsync();
        }

        // Get order items by vendorId (Vendor-Specific)
        public async Task<Order> GetOrderByVendorId(string orderId, string vendorId)
        {
            var order = await _orders.Find(o => o.OrderId == orderId).FirstOrDefaultAsync();

            if (order == null)
            {
                return null;
            }

            var vendorSpecificItems = order.OrderItems
                .Where(item => item.VendorId == vendorId)
                .ToList();

            if (!vendorSpecificItems.Any())
            {
                return null;
            }

            order.OrderItems = vendorSpecificItems;

            return order;
        }

    }
}
