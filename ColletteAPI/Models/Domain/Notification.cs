using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ColletteAPI.Models.Domain
{
    public class Notification
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string NotificationId { get; set; }

        public bool IsVisibleToCustomer { get; set; }
        public bool IsVisibleToVendor { get; set; }
        public bool IsVisibleToAdmin { get; set; }
        public bool IsVisibleToCSR { get; set; }
        public string Message { get; set; }
        public bool IsResolved { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? CustomerId { get; set; }
    }
}
