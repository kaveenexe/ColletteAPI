using ColletteAPI.Models.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ColletteAPI.Repositories
{
    public interface INotificationRepository
    {
        // Add a new notification
        Task AddNotification(Notification notification);

        // Get unresolved notifications for CSR
        Task<List<Notification>> GetNotificationsForCSR();

        // Mark a notification as seen by CSR
        Task MarkNotificationAsSeen(string notificationId);

        // Get unresolved notification by message
        Task<Notification> GetNotificationByMessage(string message);

        // Update the notification
        Task UpdateNotification(string id, Notification notification);
    }
}
