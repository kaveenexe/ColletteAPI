namespace ColletteAPI.Models.Dtos
{
    public class InventoryDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public bool IsLowStockAlert { get; set; }
    }

    public class CreateInventoryDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class UpdateInventoryDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
