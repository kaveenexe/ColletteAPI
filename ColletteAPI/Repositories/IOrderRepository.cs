using ColletteAPI.Models.Domain;
using ColletteAPI.Models.Dtos;

namespace ColletteAPI.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllOrders();
        Task<Order> GetOrderById(string id);
        Task<IEnumerable<Order>> GetOrdersByCustomerId(string customerId);
        Task<Order> GetOrderByCustomerIdAndOrderId(string customerId, string orderId);
        Task<Order> CreateOrderByCustomer(Order order);
        Task<Order> CreateOrderByAdmin(Order order);
        Task<bool> UpdateOrder(Order order);
        Task<bool> UpdateOrderStatus(string id, OrderStatus status);
        Task<bool> CancelOrder(string id);
        Task<bool> RequestOrderCancellation(OrderCancellationDto cancellationDto);
        Task<List<Order>> GetOrdersByStatus(OrderStatus status);
        Task<List<Order>> GetOrdersByCancelRequestStatus(CancelRequestStatus cancelRequestStatus);
        Task<bool> DeleteOrder(string id);
        Task<string> GetOrderStatus(string id);
        Task<bool> OrderExists(string orderId);
    }
}
