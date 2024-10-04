using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ColletteAPI.Services;
using ColletteAPI.Models.Dtos;
using System.Threading.Tasks;

namespace ColletteAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // Creates a new order by admin.
        [HttpPost("Admin")]
        public async Task<IActionResult> CreateOrderByAdmin([FromBody] OrderCreateDto orderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (orderDto.BillingDetails == null)
            {
                return BadRequest("Billing details are required.");
            }

            orderDto.CreatedByCustomer = false;
            orderDto.CreatedByAdmin = true;

            var result = await _orderService.CreateOrderByAdmin(orderDto);
            return CreatedAtAction(nameof(GetOrderById), new { id = result.Id }, result);
        }

        // Creates a new order by customer.
        [HttpPost("Customer")]
        public async Task<IActionResult> CreateOrderByCustomer([FromBody] OrderCreateDto orderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (orderDto.BillingDetails == null)
            {
                return BadRequest("Billing details are required.");
            }

            orderDto.CreatedByCustomer = true;
            orderDto.CreatedByAdmin = false;

            var result = await _orderService.CreateOrderByCustomer(orderDto);
            return CreatedAtAction(nameof(GetOrderById), new { id = result.Id }, result);
        }

        // Retrieves all orders.
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _orderService.GetAllOrders();
            return Ok(orders);
        }

        // Retrieves an order by its ID.
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(string id)
        {
            var order = await _orderService.GetOrderById(id);
            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        // Retrieves orders by customer ID.
        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetOrdersByCustomerId(string customerId)
        {
            var orders = await _orderService.GetOrdersByCustomerId(customerId);
            if (orders == null || !orders.Any())
            {
                return NotFound();
            }

            return Ok(orders);
        }

        // Retrieves an order by customer ID and order ID.
        [HttpGet("customer/{customerId}/order/{orderId}")]
        public async Task<IActionResult> GetOrderByCustomerIdAndOrderId(string customerId, string orderId)
        {
            var order = await _orderService.GetOrderByCustomerIdAndOrderId(customerId, orderId);
            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        // Updates the status of an existing order.
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(string id, [FromBody] OrderUpdateDto orderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _orderService.UpdateOrderStatus(id, orderDto);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        // Deletes an order by its ID.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(string id)
        {
            var result = await _orderService.DeleteOrder(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        // Order cancellation request
        [HttpPost("request-cancel")]
        public async Task<IActionResult> RequestOrderCancellation([FromBody] OrderCancellationDto cancellationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool result = await _orderService.RequestOrderCancellation(cancellationDto);

            if (result)
            {
                return Ok(new { Message = "Cancellation request submitted successfully." });
            }

            return NotFound(new { Message = "Order not found or already cancelled." });
        }

        // Cancels an existing order.
        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> CancelOrder(string id, [FromBody] OrderCancellationDto cancellationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cancellationDto.Id)
            {
                return BadRequest(new { message = "The order ID in the route does not match the order ID in the body." });
            }

            var result = await _orderService.CancelOrder(cancellationDto);

            if (!result)
            {
                return NotFound(new { message = "Order not found or already cancelled." });
            }

            return Ok(new { message = "Order cancelled successfully." });
        }

        // Retrieves the status of an order.
        [HttpGet("{id}/status")]
        public async Task<IActionResult> GetOrderStatus(string id)
        {
            var status = await _orderService.GetOrderStatus(id);
            if (status == "Order not found")
            {
                return NotFound();
            }

            return Ok(status);
        }

        // Get all pending cancellation requests
        [HttpGet("pending-cancellations")]
        public async Task<IActionResult> GetPendingCancellationRequests()
        {
            try
            {
                var pendingCancellationRequests = await _orderService.GetPendingCancellationRequests();

                if (pendingCancellationRequests == null || !pendingCancellationRequests.Any())
                {
                    return NotFound(new { Message = "No pending cancellation requests found." });
                }

                return Ok(pendingCancellationRequests);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving the data.", Error = ex.Message });
            }
        }

        // Vendor to mark products as delivered
        [HttpPut("{orderId}/vendors/{vendorId}/mark-delivered")]
        public async Task<IActionResult> MarkProductAsDelivered(string orderId, string vendorId)
        {
            bool result = await _orderService.MarkProductAsDelivered(orderId, vendorId);
            if (!result)
            {
                return NotFound(new { message = "Order not found or already delivered." });
            }

            return Ok(new { message = "Product marked as delivered successfully." });
        }

        // CSR/Admin to mark entire order as delivered
        [HttpPost("{orderId}/mark-delivered")]
        public async Task<IActionResult> MarkOrderAsDeliveredByAdmin(string orderId)
        {
            bool result = await _orderService.MarkOrderAsDeliveredByAdmin(orderId);
            if (!result)
            {
                return NotFound(new { message = "Order not found or already delivered." });
            }

            return Ok(new { message = "Order marked as delivered successfully." });
        }

        // Get order items by vendorId (Vendor-Specific)
        [HttpGet("vendor/{orderId}/{vendorId}")]
        public async Task<IActionResult> GetOrderByVendorId(string orderId, string vendorId)
        {
            var orderDto = await _orderService.GetOrderByVendorId(orderId, vendorId);

            if (orderDto == null)
            {
                return NotFound(new { message = "Order not found or no items for the specified vendor." });
            }

            return Ok(orderDto);
        }

        // Get all orders by vendorId (Vendor-Specific)
        [HttpGet("vendor/{vendorId}")]
        public async Task<IActionResult> GetOrdersByVendorId(string vendorId)
        {
            if (string.IsNullOrEmpty(vendorId))
            {
                return BadRequest(new { message = "Vendor ID cannot be null or empty." });
            }

            var orders = await _orderService.GetOrdersByVendorId(vendorId);

            if (orders == null || !orders.Any())
            {
                return NotFound(new { message = "No orders found for the specified vendor." });
            }

            return Ok(orders);
        }

        // comment
    }
}
