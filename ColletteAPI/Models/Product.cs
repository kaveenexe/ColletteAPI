using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ColletteAPI.Models
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("UniqueProductId")]
        [Required]
        public string UniqueProductId { get; set; } = string.Empty;

        [BsonElement("Name")]
        [Required]
        public string Name { get; set; } = string.Empty;

        [BsonElement("Description")]
        public string? Description { get; set; }

        [BsonElement("Price")]
        [BsonRepresentation(BsonType.Decimal128)]
        [Required]
        public decimal Price { get; set; }

        [BsonElement("Stock")]
        [Required]
        public int StockQuantity { get; set; }

        [BsonElement("VendorId")]
        [Required]
        public string VendorId { get; set; } = string.Empty;

        [BsonElement("IsActive")]
        public bool IsActive { get; set; }

        [BsonElement("Category")]
        [BsonRepresentation(BsonType.String)]
        [Required]
        public string Category { get; set; }

        [BsonElement("ImageUrl")]
        public string? ImageUrl { get; set; }
    }
}