using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ColletteAPI.Models.Domain
{
    // Define a specific enumeration for order statuses
    public enum OrderStatus
    {
        Purchased,
        Accepted,
        Processing,
        Delivered,
        Cancelled,
    }

    public enum PaymentMethods
    {
        Visa,
        MasterCard,
        COD,
    }

    // Order details
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("OrderId")]
        [Required(ErrorMessage = "Order ID is required.")]
        public string OrderId { get; set; }

        [BsonElement("OrderDate")]
        [Required(ErrorMessage = "Order date is required.")]
        public DateTime OrderDate { get; set; }

        [BsonElement("PaymentMethod")]
        [BsonRepresentation(BsonType.String)]
        public PaymentMethods PaymentMethod { get; set; }

        [BsonElement("Status")]
        [BsonRepresentation(BsonType.String)]
        [Required(ErrorMessage = "Order status is required.")]
        public OrderStatus Status { get; set; }

        [BsonElement("CustomerId")]
        public string? CustomerId { get; set; }

        [BsonElement("CreatedByCustomer")]
        public bool CreatedByCustomer { get; set; }

        [BsonElement("CreatedByAdmin")]
        public bool CreatedByAdmin { get; set; }

        [BsonElement("OrderItems")]
        [Required(ErrorMessage = "Order items are required.")]
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        [BsonElement("BillingDetails")]
        public BillingDetails? BillingDetails { get; set; }

        [BsonElement("OrderCancellation")]
        public OrderCancellation? OrderCancellation { get; set; }

        // Method to validate the order
        public void Validate()
        {
            if (OrderItems == null || !OrderItems.Any())
            {
                throw new ValidationException("An order must have at least one item.");
            }

            if (OrderDate > DateTime.Now)
            {
                throw new ValidationException("Order date cannot be in the future.");
            }
        }
    }

    // Wrapper class for order items
    public class OrderItemGroup
    {
        [BsonElement("ListItemId")]
        public int ListItemId { get; set; }

        [BsonElement("Items")]
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
    }

    // Details in an order item
    public class OrderItem
    {
        [BsonElement("ListItemId")]
        public int ListItemId { get; set; }

        [BsonElement("OrderId")]
        public string OrderId { get; set; }

        [BsonElement("ProductId")]
        public string ProductId { get; set; }

        [BsonElement("ProductName")]
        [Required(ErrorMessage = "Product name is required.")]
        public string ProductName { get; set; }

        [BsonElement("Quantity")]
        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        [BsonElement("Price")]
        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }
    }

    // Billing details for manual orders created by admin
    public class BillingDetails
    {
        [BsonElement("CustomerName")]
        [Required(ErrorMessage = "Customer name is required.")]
        public string CustomerName { get; set; }

        [BsonElement("Email")]
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [BsonElement("Phone")]
        public string Phone { get; set; }

        [BsonElement("SingleBillingAddress")]
        public string SingleBillingAddress { get; set; }

        [BsonElement("BillingAddress")]
        public BillingAddress BillingAddress { get; set; }
    }

    // Detailed billing address structure
    public class BillingAddress
    {
        [BsonElement("StreetAddress")]
        [Required(ErrorMessage = "Street address is required.")]
        public string StreetAddress { get; set; }

        [BsonElement("City")]
        [Required(ErrorMessage = "City is required.")]
        public string City { get; set; }

        [BsonElement("Province")]
        [Required(ErrorMessage = "Province is required.")]
        public string Province { get; set; }

        [BsonElement("PostalCode")]
        [Required(ErrorMessage = "Postal code is required.")]
        public string PostalCode { get; set; }

        [BsonElement("Country")]
        [Required(ErrorMessage = "Country is required.")]
        public string Country { get; set; }
    }

    // Cancellation class for storing cancellation-specific information
    public class OrderCancellation
    {
        [BsonElement("Id")]
        public string Id { get; set; }

        [BsonElement("OrderId")]
        public string OrderId { get; set; }

        [BsonElement("CancellationApproved")]
        public bool CancellationApproved { get; set; }

        [BsonElement("AdminNote")]
        public string AdminNote { get; set; }

        [BsonElement("CancellationDate")]
        public DateTime? CancellationDate { get; set; }
    }
}
