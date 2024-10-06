// Comment.cs
// Domain model representing a comment for a vendor.

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ColletteAPI.Models.Domain
{
    public class Comment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string VendorId { get; set; }
        public string CustomerId { get; set; }
        public string CommentText { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
