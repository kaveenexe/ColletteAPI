using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ColletteAPI.Models
{
    public class ProductCreateDto
    {
        [Required]
        public string UniqueProductId { get; set; } = string.Empty;

        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int StockQuantity { get; set; }

        public bool IsActive { get; set; }

        [Required]
        public string? Category { get; set; }

        public IFormFile? Image { get; set; }
    }
}