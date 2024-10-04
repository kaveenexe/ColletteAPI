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
    }
}
