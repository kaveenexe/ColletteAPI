using ColletteAPI.Models.Domain;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ColletteAPI.Models.Dtos
{
    // Data Transfer Object for Order, used for retrieving order details
    public class OrderDto
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string OrderId { get; set; }

        [Required]
        public OrderStatus Status { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public PaymentMethods PaymentMethod { get; set; }

        [Required]
        public List<OrderItemGroupDto> OrderItemsGroups { get; set; }

        public string? CustomerId { get; set; }

        public BillingDetailsDto? BillingDetails { get; set; }

        public OrderCancellationDto? OrderCancellation { get; set; }
    }

    // Data Transfer Object for creating a new order
    public class OrderCreateDto
    {
        [Required]
        public List<OrderItemGroupDto> OrderItemsGroups { get; set; }

        [Required]
        public PaymentMethods PaymentMethod { get; set; }

        public string? CustomerId { get; set; }

        public bool? CreatedByCustomer { get; set; } = false;

        public bool? CreatedByAdmin { get; set; } = false;

        public BillingDetailsDto? BillingDetails { get; set; }
    }

    // Data Transfer Object for updating an existing order
    public class OrderUpdateDto
    {
        [Required]
        public OrderStatus Status { get; set; }
    }

    // Data Transfer Object for Order Item Groups
    public class OrderItemGroupDto
    {
        [Required]
        public int ListItemId { get; set; }

        [Required]
        public List<OrderItemDto> Items { get; set; } 
    }

    // Data Transfer Object for Order Items
    public class OrderItemDto
    {
        [Required]
        public string ProductId { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        [Range(1, 50)]
        public int Quantity { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }
    }

    // Data Transfer Object for Billing Details
    public class BillingDetailsDto
    {
        [Required]
        public string CustomerName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Phone { get; set; }

        public string SingleBillingAddress { get; set; }

        public BillingAddressDto BillingAddress { get; set; }
    }

    // Data Transfer Object for Billing Address
    public class BillingAddressDto
    {
        [Required]
        public string StreetAddress { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Province { get; set; }

        [Required]
        public string PostalCode { get; set; }

        [Required]
        public string Country { get; set; }
    }

    // Data Transfer Object for Cancellation details
    public class OrderCancellationDto
    {
        [Required]
        public string Id { get; set; }

        public bool CancellationApproved { get; set; }

        public string AdminNote { get; set; }

        public DateTime? CancellationDate { get; set; }
    }
}
