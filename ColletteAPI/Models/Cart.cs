using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ColletteAPI.Models
{

    //Deifining Cart model
    public class Cart
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("UserId")]
        public string UserId { get; set; }

        [BsonElement("Items")]
        public List<CartItem> Items { get; set; } = new List<CartItem>();

        [BsonElement("TotalPrice")]
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal TotalPrice { get; set; }
    }

    public class CartItem
    {
        [BsonElement("ProductId")]
        [Required]
        public string ProductId { get; set; }

        [BsonElement("ProductName")]
        [Required]
        public string ProductName { get; set; }

        [BsonElement("Quantity")]
        [Required]
        public int Quantity { get; set; }

        [BsonElement("Price")]
        [Required]
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Price { get; set; }
    }
}