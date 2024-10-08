using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace ColletteAPI.Models.Domain
{
    // Enumeration representing the possible statuses of an order.
    public enum OrderStatus
    {
        Purchased,
        Accepted,
        Processing,
        Delivered,
        PartiallyDelivered,
        Cancelled,
        Pending
    }

    // Enumeration representing the possible statuses of a product.
    public enum ProductStatus
    {
        Purchased,
        Ready,
        Delivered
    }

    // Enumeration for defining different payment methods.
    public enum PaymentMethod // Changed to singular for consistency
    {
        Visa,
        Master,
        COD
    }

    // Enumeration for handling order cancellation request statuses.
    public enum CancelRequestStatus
    {
        Pending,
        Accepted,
        Rejected
    }

    // Class representing an Order, containing order details and items.
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString(); // Unique identifier for the order.

        [BsonElement("OrderId")]
        [Required(ErrorMessage = "Order ID is required.")]
        public string OrderId { get; set; } // Unique ID for the order.

        [BsonElement("OrderDate")]
        [Required(ErrorMessage = "Order date is required.")]
        public DateTime OrderDate { get; set; } // Date when the order was created.

        [BsonElement("PaymentMethod")]
        [BsonRepresentation(BsonType.String)]
        public PaymentMethod? PaymentMethod { get; set; } // Method of payment chosen by the customer.

        [BsonElement("Status")]
        [BsonRepresentation(BsonType.String)]
        [Required(ErrorMessage = "Order status is required.")]
        public OrderStatus Status { get; set; } // Current status of the order.

        [BsonElement("CustomerId")]
        public string? CustomerId { get; set; } // ID of the customer who placed the order.

        [BsonElement("CreatedByCustomer")]
        public bool CreatedByCustomer { get; set; } // Flag to indicate if the order was created by the customer.

        [BsonElement("CreatedByAdmin")]
        public bool CreatedByAdmin { get; set; } // Flag to indicate if the order was created by an admin.

        [BsonElement("OrderItems")]
        [Required(ErrorMessage = "Order items are required.")]
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>(); // List of items included in the order.

        [BsonElement("BillingDetails")]
        public BillingDetails? BillingDetails { get; set; } // Optional billing details for the order.

        [BsonElement("OrderCancellation")]
        public OrderCancellation? OrderCancellation { get; set; } // Optional cancellation details if applicable.

        [BsonElement("TotalAmount")]
        public decimal TotalAmount { get; set; } // Total amount for the order.

        // Method to validate the order before processing.
        public void Validate()
        {
            if (OrderItems == null || !OrderItems.Any())
            {
                throw new ValidationException("An order must have at least one item."); // Ensure at least one item exists.
            }

            if (OrderDate > DateTime.Now)
            {
                throw new ValidationException("Order date cannot be in the future."); // Prevent future order dates.
            }

            if (TotalAmount < 0)
            {
                throw new ValidationException("Total amount cannot be negative."); // Validate total amount.
            }
        }
    }

    // Wrapper class for grouping order items.
    public class OrderItemGroup
    {
        [BsonElement("ListItemId")]
        public int ListItemId { get; set; } // Identifier for the list of items.

        [BsonElement("Items")]
        public List<OrderItem> Items { get; set; } = new List<OrderItem>(); // List of order items in this group.
    }

    // Class representing details of an item within an order.
    public class OrderItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString(); // Unique identifier for the order item.

        [BsonElement("ListItemId")]
        public int ListItemId { get; set; } // Identifier for the list item.

        [BsonElement("OrderId")]
        public string OrderId { get; set; } // ID of the order this item belongs to.

        [BsonElement("ProductId")]
        public string ProductId { get; set; } // ID of the product.

        [BsonElement("ProductName")]
        [Required(ErrorMessage = "Product name is required.")]
        public string ProductName { get; set; } // Name of the product.

        [BsonElement("VendorId")]
        public string VendorId { get; set; } // ID of the vendor supplying the product.

        [BsonElement("Quantity")]
        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; } // Quantity of the product ordered.

        [BsonElement("Price")]
        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; } // Price of the product.

        [BsonElement("ProductStatus")]
        [BsonRepresentation(BsonType.String)]
        [Required(ErrorMessage = "Product status is required.")]
        public ProductStatus ProductStatus { get; set; } // Status of the product in the order.
    }

    // Class representing billing details for an order.
    public class BillingDetails
    {
        [BsonElement("CustomerName")]
        [Required(ErrorMessage = "Customer name is required.")]
        public string CustomerName { get; set; } // Name of the customer.

        [BsonElement("Email")]
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } // Customer's email address.

        [BsonElement("Phone")]
        public string Phone { get; set; } // Customer's phone number.

        [BsonElement("SingleBillingAddress")]
        public string SingleBillingAddress { get; set; } // Customer's billing address as a single line.

        [BsonElement("BillingAddress")]
        public BillingAddress BillingAddress { get; set; } // Detailed billing address structure.
    }

    // Class representing the detailed structure of a billing address.
    public class BillingAddress
    {
        [BsonElement("StreetAddress")]
        [Required(ErrorMessage = "Street address is required.")]
        public string StreetAddress { get; set; } // Street address of the customer.

        [BsonElement("City")]
        [Required(ErrorMessage = "City is required.")]
        public string City { get; set; } // City of the customer.

        [BsonElement("Province")]
        [Required(ErrorMessage = "Province is required.")]
        public string Province { get; set; } // Province of the customer.

        [BsonElement("PostalCode")]
        [Required(ErrorMessage = "Postal code is required.")]
        public string PostalCode { get; set; } // Postal code of the customer.

        [BsonElement("Country")]
        [Required(ErrorMessage = "Country is required.")]
        public string Country { get; set; } // Country of the customer.
    }

    // Class representing order cancellation details.
    public class OrderCancellation
    {
        [BsonElement("Id")]
        public string Id { get; set; } // Unique identifier for the cancellation.

        [BsonElement("OrderId")]
        public string OrderId { get; set; } // ID of the order being canceled.

        [BsonElement("CancellationApproved")]
        public bool CancellationApproved { get; set; } // Flag indicating if cancellation is approved.

        [BsonElement("CancellationDate")]
        public DateTime? CancellationDate { get; set; } // Date of cancellation if approved.

        [BsonElement("CancelRequestStatus")]
        [BsonRepresentation(BsonType.String)]
        public CancelRequestStatus CancelRequestStatus { get; set; } // Status of the cancellation request.
    }
}
