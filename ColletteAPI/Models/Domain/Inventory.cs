

namespace ColletteAPI.Models.Domain
{
    public class Inventory
    {
        public int InventoryId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public bool IsLowStockAlert { get; set; }

        // Navigation Properties
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
