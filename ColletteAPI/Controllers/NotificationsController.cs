using ColletteAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ColletteAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationsController(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        [HttpGet("csr")]
        public async Task<IActionResult> GetNotificationsForCSR()
        {
            var notifications = await _notificationRepository.GetNotificationsForCSR();
            return Ok(notifications);
        }

        [HttpGet("admin")]
        public async Task<IActionResult> GetNotificationsForAdmin()
        {
            var notifications = await _notificationRepository.GetNotificationsForAdmin();
            return Ok(notifications);
        }

        [HttpGet("resolved/{customerId}")]
        public async Task<IActionResult> GetResolvedNotificationsByCustomerId(string customerId)
        {
            var notifications = await _notificationRepository.GetResolvedNotificationsByCustomerId(customerId);
            if (notifications == null || !notifications.Any())
            {
                return NotFound(new { message = "No resolved notifications found for this customer." });
            }
            return Ok(notifications);
        }
    }
}
