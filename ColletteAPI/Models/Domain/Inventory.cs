using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ColletteAPI.Models.Domain
{
    public class Inventory
    {
        [BsonId]  // Marks this as the MongoDB ID field
        [BsonRepresentation(BsonType.ObjectId)]  // Ensures MongoDB can convert between ObjectId and string
        public string Id { get; set; }  // MongoDB's ObjectId

        [BsonElement("InventoryId")]
        public string InventoryId { get; set; }

        [BsonElement("ProductId")]
        public string ProductId { get; set; }

        [BsonElement("ProductName")]
        public string ProductName { get; set; }

        [BsonElement("Quantity")]
        public int Quantity { get; set; }

        [BsonElement("IsLowStockAlert")]
        public bool IsLowStockAlert { get; set; }

        // Navigation Properties
        // (if Product is defined as a separate entity and you want to include it, otherwise you can comment/remove this)
        // [BsonIgnore] // Uncomment if you don't want it to be serialized in MongoDB
        public Product Product { get; set; }

        // Methods for stock management
        public void AddStock(int quantity)
        {
            Quantity += quantity;
            CheckLowStock();
        }

        public void RemoveStock(int quantity)
        {
            if (Quantity - quantity >= 0)
            {
                Quantity -= quantity;
            }
            CheckLowStock();
        }

        private void CheckLowStock()
        {
            IsLowStockAlert = Quantity <= 5;
        }
    }
}
