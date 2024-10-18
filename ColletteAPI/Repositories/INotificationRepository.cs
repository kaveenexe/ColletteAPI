using ColletteAPI.Models.Domain;
using Microsoft.AspNetCore.Mvc;
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

        // Get unresolved notifications for Admin
        Task<List<Notification>> GetNotificationsForAdmin();

        // Get resolved notifications for a Particular Customer
        Task<List<Notification>> GetResolvedNotificationsByCustomerId(string customerId);

        // Mark a notification as seen by CSR
        Task MarkNotificationAsSeen(string notificationId);

        // Get unresolved notification by message
        Task<Notification> GetNotificationByMessage(string message);

        // Update the notification
        Task UpdateNotification(string id, Notification notification);

        Task<List<Notification>> GetNotificationsByVendorId(string vendorId);
    }
}
