// Inventory.cs
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ColletteAPI.Models.Domain
{
    // Represents an inventory item in the system
    public class Inventory
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("productId")]
        public string ProductId { get; set; }  // Product associated with inventory

        [BsonElement("vendorId")]
        public string VendorId { get; set; }

        [BsonElement("Stock")]
        public int StockQuantity { get; set; }  // Quantity available in stock

        [BsonElement("lowStockThreshold")]
        public int LowStockThreshold { get; set; } = 5;  // Alert threshold for low stock
    }
}
