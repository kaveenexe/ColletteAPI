using ColletteAPI.Models.Dtos;

namespace ColletteAPI.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetAllOrders();
        Task<OrderDto> GetOrderById(string id);
        Task<OrderDto> CreateOrder(OrderCreateDto orderDto);
        Task<bool> UpdateOrderStatus(string id, OrderUpdateDto orderDto);
        Task<bool> DeleteOrder(string id);
        Task<bool> CancelOrder(string id, string adminNote);
        Task<bool> MarkOrderAsDelivered(string id);
        Task<string> GetOrderStatus(string id);
    }
}
