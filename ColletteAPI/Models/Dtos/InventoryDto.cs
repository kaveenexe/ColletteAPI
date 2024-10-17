// InventoryDtos.cs
namespace ColletteAPI.Models.Dtos
{
    // DTO for retrieving inventory details
    public class InventoryDto
    {
        public string Id { get; set; }  // Inventory ID
        public string ProductId { get; set; }  // Product associated with the inventory
        public string ProductName { get; set; }  // Name of the product
        public int StockQuantity { get; set; }  // Quantity available in stock

        public string Category { get; set; }  // Product category
    }
}
