using ColletteAPI.Models.Dtos;

namespace ColletteAPI.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetAllOrders();
        Task<OrderDto> GetOrderById(string id);
        Task<IEnumerable<OrderDto>> GetOrdersByCustomerId(string customerId);
        Task<OrderDto> GetOrderByCustomerIdAndOrderId(string customerId, string orderId);
        Task<OrderDto> CreateOrderByCustomer(OrderCreateDto orderDto);
        Task<OrderDto> CreateOrderByAdmin(OrderCreateDto orderDto);
        Task<bool> UpdateOrderStatus(string id, OrderUpdateDto orderDto);
        Task<bool> DeleteOrder(string id);
        Task<bool> RequestOrderCancellation(OrderCancellationDto cancellationDto);
        Task<bool> CancelOrder(OrderCancellationDto cancellationDto);
        Task<string> GetOrderStatus(string id);
        Task<List<OrderDto>> GetPendingCancellationRequests();
        Task<bool> MarkProductAsDelivered(string orderId, string vendorId);
        Task<bool> MarkOrderAsDeliveredByAdmin(string orderId);
    }
}
