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

        // Retrieves all orders.
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _orderService.GetAllOrders();
            return Ok(orders);
        }

        // Creates a new order.
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDto orderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _orderService.CreateOrder(orderDto);
            return CreatedAtAction(nameof(GetOrderById), new { id = result.Id }, result);
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

        // Cancels an existing order.
        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> CancelOrder(string id, [FromBody] string adminNote)
        {
            if (string.IsNullOrWhiteSpace(adminNote))
            {
                return BadRequest("Admin note is required for cancellation.");
            }

            var result = await _orderService.CancelOrder(id, adminNote);
            if (!result)
            {
                return NotFound();
            }

            return NoContent(); 
        }

        // Marks an order as delivered.
        [HttpPost("{id}/deliver")]
        public async Task<IActionResult> MarkOrderAsDelivered(string id)
        {
            var result = await _orderService.MarkOrderAsDelivered(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent(); 
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
    }
}
