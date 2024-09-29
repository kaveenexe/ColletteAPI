using ColletteAPI.Models.Domain;
using ColletteAPI.Models.Dtos;
using ColletteAPI.Repositories;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ColletteAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;

        public OrderService(IOrderRepository orderRepository, IProductRepository productRepository, IUserRepository userRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<OrderDto>> GetAllOrders()
        {
            var orders = await _orderRepository.GetAllOrders();
            return orders.Select(o => new OrderDto
            {
                Id = o.Id,
                OrderId = o.OrderId,
                Status = o.Status,
                OrderDate = o.OrderDate,
                PaymentMethod = o.PaymentMethod,
                OrderItemsGroups = o.OrderItems.GroupBy(oi => oi.ListItemId)
                    .Select(group => new OrderItemGroupDto
                    {
                        ListItemId = group.Key,
                        Items = group.Select(oi => new OrderItemDto
                        {
                            ProductId = oi.ProductId,
                            ProductName = oi.ProductName,
                            Quantity = oi.Quantity,
                            Price = oi.Price
                        }).ToList()
                    }).ToList()
            }).ToList();
        }

        public async Task<OrderDto> GetOrderById(string id)
        {
            var order = await _orderRepository.GetOrderById(id);
            if (order == null)
            {
                return null;
            }

            return new OrderDto
            {
                Id = order.Id,
                OrderId = order.OrderId,
                Status = order.Status,
                OrderDate = order.OrderDate,
                PaymentMethod = order.PaymentMethod,
                OrderItemsGroups = order.OrderItems.GroupBy(oi => oi.ListItemId)
                    .Select(group => new OrderItemGroupDto
                    {
                        ListItemId = group.Key,
                        Items = group.Select(oi => new OrderItemDto
                        {
                            ProductId = oi.ProductId,
                            ProductName = oi.ProductName,
                            Quantity = oi.Quantity,
                            Price = oi.Price
                        }).ToList()
                    }).ToList()
            };
        }

        public async Task<OrderDto> CreateOrderByCustomer(OrderCreateDto orderDto)
        {
            var customer = await _userRepository.GetUserById(orderDto.CustomerId);
            if (customer == null)
            {
                throw new ValidationException($"Customer with ID {orderDto.CustomerId} not found.");
            }

            string orderId = await GenerateUniqueOrderId();

            var orderItems = new List<OrderItem>();

            foreach (var orderItemGroup in orderDto.OrderItemsGroups)
            {
                foreach (var item in orderItemGroup.Items)
                {
                    var product = await _productRepository.GetProductById(item.ProductId);
                    if (product == null)
                    {
                        throw new ValidationException($"Product with ID {item.ProductId} not found.");
                    }

                    orderItems.Add(new OrderItem
                    {
                        ListItemId = orderItemGroup.ListItemId,
                        OrderId = orderId,
                        ProductId = item.ProductId,
                        ProductName = product.Name,
                        Quantity = item.Quantity,
                        Price = product.Price
                    });
                }
            }

            var customerFullName = $"{customer.FirstName} {customer.LastName}";

            BillingDetails billingDetails = null;
            if (orderDto.BillingDetails != null && !string.IsNullOrEmpty(orderDto.BillingDetails.SingleBillingAddress))
            {
                billingDetails = new BillingDetails
                {
                    CustomerName = customerFullName,
                    Email = customer.Email,
                    SingleBillingAddress = customer.Address
                };
            }

            var order = new Order
            {
                OrderId = orderId,
                OrderDate = DateTime.Now,
                PaymentMethod = orderDto.PaymentMethod,
                Status = OrderStatus.Purchased,
                OrderItems = orderItems,
                CustomerId = orderDto.CustomerId,
                CreatedByCustomer = true,
                BillingDetails = billingDetails
            };

            var createdOrder = await _orderRepository.CreateOrderByCustomer(order);

            return new OrderDto
            {
                Id = createdOrder.Id,
                OrderId = createdOrder.OrderId,
                Status = createdOrder.Status,
                OrderDate = createdOrder.OrderDate,
                PaymentMethod = createdOrder.PaymentMethod,
                OrderItemsGroups = createdOrder.OrderItems.GroupBy(oi => oi.ListItemId)
                    .Select(group => new OrderItemGroupDto
                    {
                        ListItemId = group.Key,
                        Items = group.Select(oi => new OrderItemDto
                        {
                            ProductId = oi.ProductId,
                            ProductName = oi.ProductName,
                            Quantity = oi.Quantity,
                            Price = oi.Price
                        }).ToList()
                    }).ToList(),
                CustomerId = createdOrder.CustomerId,
                BillingDetails = createdOrder.BillingDetails != null ? new BillingDetailsDto
                {
                    CustomerName = createdOrder.BillingDetails.CustomerName,
                    Email = createdOrder.BillingDetails.Email,
                    SingleBillingAddress = createdOrder.BillingDetails.SingleBillingAddress
                } : null
            };
        }

        public async Task<OrderDto> CreateOrderByAdmin(OrderCreateDto orderDto)
        {
            BillingDetails? billingDetails = null;
            if (orderDto.CreatedByAdmin == true && orderDto.BillingDetails != null)
            {
                billingDetails = new BillingDetails
                {
                    CustomerName = orderDto.BillingDetails.CustomerName,
                    Email = orderDto.BillingDetails.Email,
                    Phone = orderDto.BillingDetails.Phone,
                    BillingAddress = new BillingAddress
                    {
                        StreetAddress = orderDto.BillingDetails.BillingAddress.StreetAddress,
                        City = orderDto.BillingDetails.BillingAddress.City,
                        Province = orderDto.BillingDetails.BillingAddress.Province,
                        PostalCode = orderDto.BillingDetails.BillingAddress.PostalCode,
                        Country = orderDto.BillingDetails.BillingAddress.Country
                    }
                };
            }

            string orderId = await GenerateUniqueOrderId();

            var orderItems = new List<OrderItem>();

            foreach (var orderItemGroup in orderDto.OrderItemsGroups)
            {
                foreach (var item in orderItemGroup.Items)
                {
                    var product = await _productRepository.GetProductById(item.ProductId);
                    if (product == null)
                    {
                        throw new ValidationException($"Product with ID {item.ProductId} not found.");
                    }

                    orderItems.Add(new OrderItem
                    {
                        ListItemId = orderItemGroup.ListItemId,
                        OrderId = orderId,
                        ProductId = item.ProductId,
                        ProductName = product.Name,
                        Quantity = item.Quantity,
                        Price = product.Price
                    });
                }
            }

            var order = new Order
            {
                OrderId = orderId,
                OrderDate = DateTime.Now,
                PaymentMethod = orderDto.PaymentMethod,
                Status = OrderStatus.Purchased,
                OrderItems = orderItems,
                CustomerId = orderDto.CreatedByCustomer == true ? orderDto.CustomerId : null,
                CreatedByCustomer = orderDto.CreatedByCustomer ?? false,
                CreatedByAdmin = orderDto.CreatedByAdmin ?? false,
                BillingDetails = billingDetails
            };

            var createdOrder = await _orderRepository.CreateOrderByAdmin(order);

            return new OrderDto
            {
                Id = createdOrder.Id,
                OrderId = createdOrder.OrderId,
                Status = createdOrder.Status,
                OrderDate = createdOrder.OrderDate,
                PaymentMethod = createdOrder.PaymentMethod,
                OrderItemsGroups = createdOrder.OrderItems.GroupBy(oi => oi.ListItemId)
                    .Select(group => new OrderItemGroupDto
                    {
                        ListItemId = group.Key,
                        Items = group.Select(oi => new OrderItemDto
                        {
                            ProductId = oi.ProductId,
                            ProductName = oi.ProductName,
                            Quantity = oi.Quantity,
                            Price = oi.Price
                        }).ToList()
                    }).ToList(),
                CustomerId = createdOrder.CreatedByCustomer == true ? createdOrder.CustomerId : null,
                BillingDetails = createdOrder.BillingDetails != null ? new BillingDetailsDto
                {
                    CustomerName = createdOrder.BillingDetails.CustomerName,
                    Email = createdOrder.BillingDetails.Email,
                    Phone = createdOrder.BillingDetails.Phone,
                    BillingAddress = new BillingAddressDto
                    {
                        StreetAddress = createdOrder.BillingDetails.BillingAddress.StreetAddress,
                        City = createdOrder.BillingDetails.BillingAddress.City,
                        Province = createdOrder.BillingDetails.BillingAddress.Province,
                        PostalCode = createdOrder.BillingDetails.BillingAddress.PostalCode,
                        Country = createdOrder.BillingDetails.BillingAddress.Country
                    }
                } : null
            };
        }

        // Method to generate unique OrderId
        private async Task<string> GenerateUniqueOrderId()
        {
            var random = new Random();
            string orderId;
            bool isUnique = false;

            do
            {
                int randomNumber = random.Next(1000, 9999);
                orderId = $"#ORD{randomNumber}";

                isUnique = !(await _orderRepository.OrderExists(orderId));
            }
            while (!isUnique);

            return orderId;
        }

        public async Task<bool> UpdateOrderStatus(string id, OrderUpdateDto orderDto)
        {
            var order = await _orderRepository.GetOrderById(id);
            if (order == null)
            {
                return false;
            }

            return await _orderRepository.UpdateOrderStatus(id, orderDto.Status);
        }

        public async Task<bool> DeleteOrder(string id)
        {
            return await _orderRepository.DeleteOrder(id);
        }

        public async Task<bool> CancelOrder(string id, string adminNote)
        {
            return await _orderRepository.CancelOrder(id, adminNote);
        }

        public async Task<bool> MarkOrderAsDelivered(string id)
        {
            return await _orderRepository.MarkOrderAsDelivered(id);
        }

        public async Task<string> GetOrderStatus(string id)
        {
            return await _orderRepository.GetOrderStatus(id);
        }
    }
}
