// Inventory.cs
// Represents an inventory item in the system.

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ColletteAPI.Models.Domain
{
    public class Inventory
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("productId")]
        public string ProductId { get; set; }

        [BsonElement("quantity")]
        public int Quantity { get; set; }
    }
}
