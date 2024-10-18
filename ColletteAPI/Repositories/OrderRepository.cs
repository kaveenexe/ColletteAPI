using ColletteAPI.Models.Domain;
using ColletteAPI.Models.Dtos;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ColletteAPI.Repositories
{
    /*
     * Class: OrderRepository
     * Implements the IOrderRepository interface for managing order-related data operations
     * using MongoDB as the database backend.
     */
    public class OrderRepository : IOrderRepository
    {
        private readonly IMongoCollection<Order> _orders; // MongoDB collection for orders

        /*
         * Constructor: OrderRepository
         * Initializes a new instance of the OrderRepository class.
         * 
         * Parameters:
         *  - client: The MongoDB client to connect to the database.
         *  - configuration: Configuration settings for database access.
         */
        public OrderRepository(IMongoClient client, IConfiguration configuration)
        {
            var database = client.GetDatabase(configuration.GetSection("MongoDB:DatabaseName").Value);
            _orders = database.GetCollection<Order>("Orders"); // Set the orders collection
        }

        /*
         * Method: GetAllOrders
         * Retrieves all orders from the database.
         * 
         * Returns:
         *  - A list of all orders.
         */
        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            return await _orders.Find(order => true).ToListAsync();
        }

        /*
         * Method: GetOrderById
         * Retrieves an order by its ID.
         * 
         * Parameters:
         *  - id: The ID of the order to retrieve.
         * 
         * Returns:
         *  - The order object, or null if not found.
         */
        public async Task<Order> GetOrderById(string id)
        {
            return await _orders.Find(order => order.Id == id).FirstOrDefaultAsync();
        }

        /*
         * Method: GetOrdersByCustomerId
         * Retrieves all orders associated with a specific customer ID.
         * 
         * Parameters:
         *  - customerId: The ID of the customer whose orders to retrieve.
         * 
         * Returns:
         *  - A list of orders belonging to the specified customer.
         */
        public async Task<IEnumerable<Order>> GetOrdersByCustomerId(string customerId)
        {
            return await _orders.Find(o => o.CustomerId == customerId).ToListAsync();
        }

        /*
         * Method: GetOrderByCustomerIdAndOrderId
         * Retrieves an order by its customer ID and order ID.
         * 
         * Parameters:
         *  - customerId: The ID of the customer.
         *  - orderId: The ID of the order to retrieve.
         * 
         * Returns:
         *  - The order object, or null if not found.
         */
        public async Task<Order> GetOrderByCustomerIdAndOrderId(string customerId, string orderId)
        {
            return await _orders.Find(order => order.CustomerId == customerId && order.Id == orderId).FirstOrDefaultAsync();
        }

        /*
         * Method: CreateOrderByCustomer
         * Inserts a new order created by a customer into the database.
         * 
         * Parameters:
         *  - order: The order object to create.
         * 
         * Returns:
         *  - The created order object.
         */
        public async Task<Order> CreateOrderByCustomer(Order order)
        {
            await _orders.InsertOneAsync(order); // Insert the order
            return order;
        }

        /*
         * Method: CreateOrderByAdmin
         * Inserts a new order created by an admin into the database.
         * 
         * Parameters:
         *  - order: The order object to create.
         * 
         * Returns:
         *  - The created order object.
         */
        public async Task<Order> CreateOrderByAdmin(Order order)
        {
            await _orders.InsertOneAsync(order); // Insert the order
            return order;
        }

        /*
         * Method: OrderExists
         * Checks if an order exists in the database by its order ID.
         * 
         * Parameters:
         *  - orderId: The ID of the order to check.
         * 
         * Returns:
         *  - True if the order exists; otherwise, false.
         */
        public async Task<bool> OrderExists(string orderId)
        {
            var order = await _orders.Find(o => o.OrderId == orderId).FirstOrDefaultAsync();
            return order != null; // Return true if order exists
        }

        /*
         * Method: UpdateOrder
         * Updates an existing order's details in the database.
         * 
         * Parameters:
         *  - order: The order object containing updated data.
         * 
         * Returns:
         *  - True if the update was successful; otherwise, false.
         */
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

            return result.IsAcknowledged && result.ModifiedCount > 0; // Return true if update was acknowledged
        }

        /*
         * Method: DeleteOrder
         * Deletes an order from the database by its ID.
         * 
         * Parameters:
         *  - id: The ID of the order to delete.
         * 
         * Returns:
         *  - True if the deletion was successful; otherwise, false.
         */
        public async Task<bool> DeleteOrder(string id)
        {
            var result = await _orders.DeleteOneAsync(o => o.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0; // Return true if deletion was acknowledged
        }

        /*
         * Method: UpdateOrderStatus
         * Updates the status of an existing order.
         * 
         * Parameters:
         *  - id: The ID of the order to update.
         *  - status: The new status to set for the order.
         * 
         * Returns:
         *  - True if the update was successful; otherwise, false.
         */
        public async Task<bool> UpdateOrderStatus(string id, OrderStatus status)
        {
            var updateDefinition = Builders<Order>.Update.Set(o => o.Status, status);
            var result = await _orders.UpdateOneAsync(o => o.Id == id, updateDefinition);
            return result.IsAcknowledged && result.ModifiedCount > 0; // Return true if update was acknowledged
        }

        /*
         * Method: RequestOrderCancellation
         * Submits a request to cancel an existing order.
         * 
         * Parameters:
         *  - cancellationDto: The data transfer object containing cancellation details.
         * 
         * Returns:
         *  - True if the cancellation request was submitted successfully; otherwise, false.
         */
        public async Task<bool> RequestOrderCancellation(OrderCancellationDto cancellationDto)
        {
            var order = await GetOrderById(cancellationDto.Id);

            if (order == null || order.Status == OrderStatus.Cancelled)
            {
                return false; // Return false if order is not found or already cancelled
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

            return result.IsAcknowledged && result.ModifiedCount > 0; // Return true if update was acknowledged
        }

        /*
         * Method: CancelOrder
         * Cancels an existing order.
         * 
         * Parameters:
         *  - id: The ID of the order to cancel.
         * 
         * Returns:
         *  - True if the cancellation was successful; otherwise, false.
         */
        public async Task<bool> CancelOrder(string id)
        {
            var order = await GetOrderById(id);
            if (order == null || order.Status == OrderStatus.Delivered || order.Status == OrderStatus.PartiallyDelivered)
            {
                return false; // Return false if order is not found or already delivered
            }

            var cancellation = new OrderCancellation
            {
                Id = ObjectId.GenerateNewId().ToString(),
                OrderId = id,
                CancellationApproved = true,
                CancellationDate = DateTime.UtcNow,
                CancelRequestStatus = CancelRequestStatus.Accepted
            };

            var updateDefinition = Builders<Order>.Update
                .Set(o => o.OrderCancellation, cancellation)
                .Set(o => o.Status, OrderStatus.Cancelled);

            var result = await _orders.UpdateOneAsync(o => o.Id == id, updateDefinition);

            return result.IsAcknowledged && result.ModifiedCount > 0; // Return true if update was acknowledged
        }

        /*
         * Method: GetOrderStatus
         * Retrieves the status of an order by its ID.
         * 
         * Parameters:
         *  - id: The ID of the order whose status to retrieve.
         * 
         * Returns:
         *  - The status of the order as a string, or "Order not found" if not found.
         */
        public async Task<string> GetOrderStatus(string id)
        {
            var order = await GetOrderById(id);
            return order?.Status.ToString() ?? "Order not found"; // Return order status or not found message
        }

        /*
         * Method: GetOrdersByStatus
         * Retrieves all orders with a specific status.
         * 
         * Parameters:
         *  - status: The status of orders to retrieve.
         * 
         * Returns:
         *  - A list of orders matching the specified status.
         */
        public async Task<List<Order>> GetOrdersByStatus(OrderStatus status)
        {
            return await _orders.Find(order => order.Status == status).ToListAsync();
        }

        /*
         * Method: GetOrdersByCancelRequestStatus
         * Retrieves all orders with a specific cancellation request status.
         * 
         * Parameters:
         *  - cancelRequestStatus: The cancellation request status to filter by.
         * 
         * Returns:
         *  - A list of orders with the specified cancellation request status.
         */
        public async Task<List<Order>> GetOrdersByCancelRequestStatus(CancelRequestStatus cancelRequestStatus)
        {
            return await _orders
                .Find(order => order.OrderCancellation != null && order.OrderCancellation.CancelRequestStatus == cancelRequestStatus)
                .ToListAsync();
        }

        /*
         * Method: GetOrderByVendorId
         * Retrieves an order by its ID, filtered by a specific vendor ID.
         * 
         * Parameters:
         *  - orderId: The ID of the order to retrieve.
         *  - vendorId: The ID of the vendor to filter the order items.
         * 
         * Returns:
         *  - The order object with vendor-specific items, or null if not found.
         */
        public async Task<Order> GetOrderByVendorId(string orderId, string vendorId)
        {
            var order = await _orders.Find(o => o.Id == orderId).FirstOrDefaultAsync();

            if (order == null)
            {
                return null; // Return null if order is not found
            }

            var vendorSpecificItems = order.OrderItems
                .Where(item => item.VendorId == vendorId)
                .ToList();

            if (!vendorSpecificItems.Any())
            {
                return null; // Return null if no vendor-specific items found
            }

            order.OrderItems = vendorSpecificItems; // Set the order items to only include vendor-specific items

            return order; // Return the filtered order
        }

        /*
         * Method: GetOrdersByVendorId
         * Retrieves all orders that contain items from a specific vendor.
         * 
         * Parameters:
         *  - vendorId: The ID of the vendor to filter orders by.
         * 
         * Returns:
         *  - A list of orders containing items from the specified vendor.
         */
        public async Task<List<Order>> GetOrdersByVendorId(string vendorId)
        {
            var orders = await _orders.Find(order => order.OrderItems.Any(item => item.VendorId == vendorId)).ToListAsync();
            return orders; // Return the list of filtered orders
        }

                /*
         * Method: GetOrdersByProductIdAsync
         * Retrieves all orders that contain a specific product.
         * 
         * Parameters:
         *  - productId: The ID of the product to filter orders by.
         * 
         * Returns:
         *  - A list of orders containing the specified product.
         * 
         * Throws:
         *  - ArgumentNullException: If the productId is null or empty.
         *  - MongoException: If there is an issue retrieving orders from the database.
         */
        public async Task<List<Order>> GetOrdersByProductId(string productId)
        {
            var filter = Builders<Order>.Filter.ElemMatch(o => o.OrderItems, item => item.ProductId == productId);
            return await _orders.Find(filter).ToListAsync();
        }
    }
}
