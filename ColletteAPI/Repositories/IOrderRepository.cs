using ColletteAPI.Models.Domain;

namespace ColletteAPI.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllOrders();
        Task<Order> GetOrderById(string id);
        Task<Order> CreateOrder(Order order);
        Task<bool> UpdateOrderStatus(string id, OrderStatus status);
        Task<bool> CancelOrder(string id, string adminNote);
        Task<bool> MarkOrderAsDelivered(string id);
        Task<bool> DeleteOrder(string id);
        Task<string> GetOrderStatus(string id);
        Task<bool> OrderExists(string orderId);
    }
}
