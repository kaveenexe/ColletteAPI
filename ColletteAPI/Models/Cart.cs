using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ColletteAPI.Models
{
    public class Cart
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string UserId { get; set; }

        public List<CartItem> Items { get; set; } = new List<CartItem>();

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal TotalPrice { get; set; }
    }

    public class CartItem
    {
        [Required]
        public string ProductId { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}