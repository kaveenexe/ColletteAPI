using ColletteAPI.Models.Domain;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ColletteAPI.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly IMongoCollection<Notification> _notifications;

        public NotificationRepository(IMongoClient client, IConfiguration configuration)
        {
            var database = client.GetDatabase(configuration.GetSection("MongoDB:DatabaseName").Value);
            _notifications = database.GetCollection<Notification>("Notifications");
        }

        public async Task AddNotification(Notification notification)
        {
            await _notifications.InsertOneAsync(notification);
        }

        public async Task<List<Notification>> GetNotificationsForCSR()
        {
            return await _notifications.Find(n => n.IsVisibleToCSR && !n.IsResolved).ToListAsync();
        }

        public async Task<List<Notification>> GetNotificationsForAdmin()
        {
            return await _notifications.Find(n => n.IsVisibleToAdmin && !n.IsResolved).ToListAsync();
        }

        public async Task<List<Notification>> GetResolvedNotificationsByCustomerId(string customerId)
        {
            return await _notifications.Find(n => n.CustomerId == customerId && n.IsResolved).ToListAsync();
        }

        public async Task MarkNotificationAsSeen(string notificationId)
        {
            var filter = Builders<Notification>.Filter.Eq(n => n.NotificationId, notificationId);
            var update = Builders<Notification>.Update.Set(n => n.IsResolved, true); // Mark as resolved
            await _notifications.UpdateOneAsync(filter, update);
        }

        public async Task<Notification> GetNotificationByMessage(string message)
        {
            return await _notifications.Find(n => n.Message == message && !n.IsResolved).FirstOrDefaultAsync();
        }

        public async Task UpdateNotification(string id, Notification notification)
        {
            var objectId = ObjectId.Parse(id);
            await _notifications.ReplaceOneAsync(n => n.NotificationId == objectId.ToString(), notification);
        }

        public async Task DeleteNotification(string id)
        {
            var objectId = ObjectId.Parse(id);
            await _notifications.DeleteOneAsync(n => n.NotificationId == objectId.ToString());
        }
    }
}
