using ColletteAPI.Models.Domain;
using ColletteAPI.Models.Dtos;
using ColletteAPI.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;
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
        private readonly INotificationRepository _notificationRepository;

        public OrderService(IOrderRepository orderRepository, IProductRepository productRepository, IUserRepository userRepository, INotificationRepository notificationRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
            _notificationRepository = notificationRepository;
        }

        // Get all orders
        public async Task<IEnumerable<OrderDto>> GetAllOrders()
        {
            var orders = await _orderRepository.GetAllOrders();
            return orders.Select(o => new OrderDto
            {
                Id = o.Id,
                OrderId = o.OrderId,
                Status = o.Status,
                OrderDate = o.OrderDate,
                PaymentMethod = o.PaymentMethod ?? PaymentMethods.Visa,
                BillingDetails = o.BillingDetails != null
                    ? new BillingDetailsDto
                    {
                        CustomerName = o.BillingDetails.CustomerName,
                        Email = o.BillingDetails.Email,
                        Phone = o.BillingDetails.Phone,
                        SingleBillingAddress = o.BillingDetails.SingleBillingAddress,
                        BillingAddress = o.BillingDetails.BillingAddress != null
                            ? new BillingAddressDto
                            {
                                StreetAddress = o.BillingDetails.BillingAddress.StreetAddress,
                                City = o.BillingDetails.BillingAddress.City,
                                Province = o.BillingDetails.BillingAddress.Province,
                                PostalCode = o.BillingDetails.BillingAddress.PostalCode,
                                Country = o.BillingDetails.BillingAddress.Country
                            }
                            : null
                    }
                    : null,
                TotalAmount = o.TotalAmount,
                CreatedByCustomer = o.CreatedByCustomer,
                CreatedByAdmin = o.CreatedByAdmin,
                OrderItemsGroups = o.OrderItems.GroupBy(oi => oi.ListItemId)
                    .Select(group => new OrderItemGroupDto
                    {
                        ListItemId = group.Key,
                        Items = group.Select(oi => new OrderItemDto
                        {
                            ProductId = oi.ProductId,
                            ProductName = oi.ProductName,
                            VendorId = oi.VendorId,
                            Quantity = oi.Quantity,
                            Price = oi.Price,
                            ProductStatus = oi.ProductStatus,
                        }).ToList()
                    }).ToList()
            }).ToList();
        }

        // Get order by id
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
                PaymentMethod = order.PaymentMethod ?? PaymentMethods.Visa,
                BillingDetails = order.BillingDetails != null
                    ? new BillingDetailsDto
                    {
                        CustomerName = order.BillingDetails.CustomerName,
                        Email = order.BillingDetails.Email,
                        Phone = order.BillingDetails.Phone,
                        SingleBillingAddress = order.BillingDetails.SingleBillingAddress,
                        BillingAddress = order.BillingDetails.BillingAddress != null
                            ? new BillingAddressDto
                            {
                                StreetAddress = order.BillingDetails.BillingAddress.StreetAddress,
                                City = order.BillingDetails.BillingAddress.City,
                                Province = order.BillingDetails.BillingAddress.Province,
                                PostalCode = order.BillingDetails.BillingAddress.PostalCode,
                                Country = order.BillingDetails.BillingAddress.Country
                            }
                            : null
                    }
                    : null,
                TotalAmount = order.TotalAmount,
                CreatedByCustomer = order.CreatedByCustomer,
                CreatedByAdmin = order.CreatedByAdmin,
                OrderItemsGroups = order.OrderItems.GroupBy(oi => oi.ListItemId)
                    .Select(group => new OrderItemGroupDto
                    {
                        ListItemId = group.Key,
                        Items = group.Select(oi => new OrderItemDto
                        {
                            ProductId = oi.ProductId,
                            ProductName = oi.ProductName,
                            VendorId = oi.VendorId,
                            Quantity = oi.Quantity,
                            Price = oi.Price,
                            ProductStatus = oi.ProductStatus,
                        }).ToList()
                    }).ToList()
            };
        }

        // Get order by customerId
        public async Task<IEnumerable<OrderDto>> GetOrdersByCustomerId(string customerId)
        {
            var orders = await _orderRepository.GetOrdersByCustomerId(customerId);

            if (orders == null || !orders.Any())
            {
                return Enumerable.Empty<OrderDto>();
            }

            // Map the orders to OrderDto
            return orders.Select(o => new OrderDto
            {
                Id = o.Id,
                OrderId = o.OrderId,
                Status = o.Status,
                OrderDate = o.OrderDate,
                PaymentMethod = o.PaymentMethod ?? PaymentMethods.Visa,
                BillingDetails = o.BillingDetails != null
                    ? new BillingDetailsDto
                    {
                        CustomerName = o.BillingDetails.CustomerName,
                        Email = o.BillingDetails.Email,
                        Phone = o.BillingDetails.Phone,
                        SingleBillingAddress = o.BillingDetails.SingleBillingAddress,
                        BillingAddress = o.BillingDetails.BillingAddress != null
                            ? new BillingAddressDto
                            {
                                StreetAddress = o.BillingDetails.BillingAddress.StreetAddress,
                                City = o.BillingDetails.BillingAddress.City,
                                Province = o.BillingDetails.BillingAddress.Province,
                                PostalCode = o.BillingDetails.BillingAddress.PostalCode,
                                Country = o.BillingDetails.BillingAddress.Country
                            }
                            : null
                    }
                    : null,
                TotalAmount = o.TotalAmount,
                CreatedByCustomer = o.CreatedByCustomer,
                CreatedByAdmin = o.CreatedByAdmin,
                OrderItemsGroups = o.OrderItems.GroupBy(oi => oi.ListItemId)
                    .Select(group => new OrderItemGroupDto
                    {
                        ListItemId = group.Key,
                        Items = group.Select(oi => new OrderItemDto
                        {
                            ProductId = oi.ProductId,
                            ProductName = oi.ProductName,
                            VendorId = oi.VendorId,
                            Quantity = oi.Quantity,
                            Price = oi.Price,
                            ProductStatus = oi.ProductStatus,
                        }).ToList()
                    }).ToList()
            }).ToList();
        }

        // Get order by customerId and orderId
        public async Task<OrderDto> GetOrderByCustomerIdAndOrderId(string customerId, string orderId)
        {
            var order = await _orderRepository.GetOrderByCustomerIdAndOrderId(customerId, orderId);

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
                PaymentMethod = order.PaymentMethod ?? PaymentMethods.Visa,
                BillingDetails = order.BillingDetails != null
                    ? new BillingDetailsDto
                    {
                        CustomerName = order.BillingDetails.CustomerName,
                        Email = order.BillingDetails.Email,
                        Phone = order.BillingDetails.Phone,
                        SingleBillingAddress = order.BillingDetails.SingleBillingAddress,
                        BillingAddress = order.BillingDetails.BillingAddress != null
                            ? new BillingAddressDto
                            {
                                StreetAddress = order.BillingDetails.BillingAddress.StreetAddress,
                                City = order.BillingDetails.BillingAddress.City,
                                Province = order.BillingDetails.BillingAddress.Province,
                                PostalCode = order.BillingDetails.BillingAddress.PostalCode,
                                Country = order.BillingDetails.BillingAddress.Country
                            }
                            : null
                    }
                    : null,
                TotalAmount = order.TotalAmount,
                CreatedByCustomer = order.CreatedByCustomer,
                CreatedByAdmin = order.CreatedByAdmin,
                OrderItemsGroups = order.OrderItems.GroupBy(oi => oi.ListItemId)
                    .Select(group => new OrderItemGroupDto
                    {
                        ListItemId = group.Key,
                        Items = group.Select(oi => new OrderItemDto
                        {
                            ProductId = oi.ProductId,
                            ProductName = oi.ProductName,
                            VendorId = oi.VendorId,
                            Quantity = oi.Quantity,
                            Price = oi.Price,
                            ProductStatus = oi.ProductStatus,
                        }).ToList()
                    }).ToList()
            };
        }

        // Create order by customer
        public async Task<OrderDto> CreateOrderByCustomer(OrderCreateDto orderDto)
        {
            var customer = await _userRepository.GetUserById(orderDto.CustomerId);
            if (customer == null)
            {
                throw new ValidationException($"Customer with ID {orderDto.CustomerId} not found.");
            }

            string orderId = await GenerateUniqueOrderId();

            var orderItems = new List<OrderItem>();
            decimal totalAmount = 0;

            foreach (var orderItemGroup in orderDto.OrderItemsGroups)
            {
                foreach (var item in orderItemGroup.Items)
                {
                    var product = await _productRepository.GetProductById(item.ProductId);
                    if (product == null)
                    {
                        throw new ValidationException($"Product with ID {item.ProductId} not found.");
                    }

                    var orderItem = new OrderItem
                    {
                        ListItemId = orderItemGroup.ListItemId,
                        OrderId = orderId,
                        ProductId = item.ProductId,
                        ProductName = product.Name,
                        VendorId = product.VendorId,
                        Quantity = item.Quantity,
                        Price = product.Price,
                        ProductStatus = ProductStatus.Purchased
                    };

                    totalAmount += orderItem.Quantity * orderItem.Price;

                    orderItems.Add(orderItem);
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
                    Phone = customer.ContactNumber,
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
                BillingDetails = billingDetails,
                TotalAmount = totalAmount
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
                            VendorId = oi.VendorId,
                            Quantity = oi.Quantity,
                            Price = oi.Price,
                            ProductStatus = oi.ProductStatus,
                        }).ToList()
                    }).ToList(),
                CustomerId = createdOrder.CustomerId,
                BillingDetails = createdOrder.BillingDetails != null ? new BillingDetailsDto
                {
                    CustomerName = createdOrder.BillingDetails.CustomerName,
                    Email = createdOrder.BillingDetails.Email,
                    Phone = createdOrder.BillingDetails.Phone,
                    SingleBillingAddress = createdOrder.BillingDetails.SingleBillingAddress
                } : null,
                TotalAmount = totalAmount
            };
        }

        // Create order by admin
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
            decimal totalAmount = 0;

            foreach (var orderItemGroup in orderDto.OrderItemsGroups)
            {
                foreach (var item in orderItemGroup.Items)
                {
                    var product = await _productRepository.GetProductById(item.ProductId);
                    if (product == null)
                    {
                        throw new ValidationException($"Product with ID {item.ProductId} not found.");
                    }

                    var orderItem = new OrderItem
                    {
                        ListItemId = orderItemGroup.ListItemId,
                        OrderId = orderId,
                        ProductId = item.ProductId,
                        ProductName = product.Name,
                        VendorId = product.VendorId,
                        Quantity = item.Quantity,
                        Price = product.Price,
                        ProductStatus = ProductStatus.Purchased
                    };

                    totalAmount += orderItem.Quantity * orderItem.Price;

                    orderItems.Add(orderItem);
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
                CreatedByAdmin = true,
                BillingDetails = billingDetails,
                TotalAmount = totalAmount
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
                            VendorId = oi.VendorId,
                            Quantity = oi.Quantity,
                            Price = oi.Price,
                            ProductStatus = oi.ProductStatus,
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
                } : null,
                TotalAmount = totalAmount
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

        // Update order status of an order
        public async Task<bool> UpdateOrderStatus(string id, OrderUpdateDto orderDto)
        {
            var order = await _orderRepository.GetOrderById(id);
            if (order == null)
            {
                return false;
            }

            bool updateResult = await _orderRepository.UpdateOrderStatus(id, orderDto.Status);

            if (updateResult && orderDto.Status == OrderStatus.Delivered)
            {
                // Create a notification for the customer
                var notification = new Notification
                {
                    Message = $"Your order {order.OrderId} has been successfully delivered.",
                    IsVisibleToCSR = false,
                    IsVisibleToAdmin = false,
                    IsVisibleToVendor = false,
                    IsVisibleToCustomer = true,
                    IsResolved = true,
                    CustomerId = order.CustomerId
                };

                await _notificationRepository.AddNotification(notification);
            }

            return updateResult;
        }

        // Delete order
        public async Task<bool> DeleteOrder(string id)
        {
            return await _orderRepository.DeleteOrder(id);
        }

        // Order cancellation request
        public async Task<bool> RequestOrderCancellation(OrderCancellationDto cancellationDto)
        {
            var order = await _orderRepository.GetOrderById(cancellationDto.Id);

            if (order == null)
            {
                throw new Exception("Order not found.");
            }

            if (order.Status == OrderStatus.Cancelled)
            {
                throw new Exception("Cannot request an order cancellation due to an already cancelled order.");
            }

            if (_notificationRepository == null)
            {
                throw new NullReferenceException("Notification repository is not initialized.");
            }

            order.OrderCancellation = new OrderCancellation
            {
                Id = ObjectId.GenerateNewId().ToString(),
                OrderId = order.OrderId,
                CancellationApproved = false,
                CancellationDate = DateTime.UtcNow,
                CancelRequestStatus = CancelRequestStatus.Pending
            };

            await _orderRepository.UpdateOrder(order);

            // Create a notification for Admin and CSR
            var notification = new Notification
            {
                Message = $"You have received a cancellation request for the order {order.OrderId}",
                IsVisibleToCSR = true,
                IsVisibleToAdmin = true,
                IsVisibleToVendor = false,
                IsVisibleToCustomer = false,
            };

            await _notificationRepository.AddNotification(notification);

            return true;
        }

        // Order Cancel
        public async Task<bool> CancelOrder(OrderCancellationDto cancellationDto)
        {
            var order = await _orderRepository.GetOrderById(cancellationDto.OrderId);

            if (order == null)
            {
                throw new Exception("Order not found.");
            }

            if (order.Status == OrderStatus.Delivered || order.Status == OrderStatus.PartiallyDelivered)
            {
                cancellationDto.CancelRequestStatus = CancelRequestStatus.Rejected;
                await RejectCancelOrder(order, order.Status == OrderStatus.Delivered ? "delivered" : "partially delivered");
                return false;
            }

            order.Status = OrderStatus.Cancelled;

            order.OrderCancellation = new OrderCancellation
            {
                Id = cancellationDto.Id,
                OrderId = order.OrderId,
                CancellationApproved = true,
                CancellationDate = DateTime.UtcNow,
                CancelRequestStatus = CancelRequestStatus.Accepted
            };

            var updateResult = await _orderRepository.UpdateOrder(order);

            if (updateResult)
            {
                // Create a notification for successful cancellation
                var notification = new Notification
                {
                    Message = $"Your order {order.OrderId} has been successfully cancelled.",
                    IsVisibleToCSR = false,
                    IsVisibleToAdmin = false,
                    IsVisibleToVendor = false,
                    IsVisibleToCustomer = true,
                    IsResolved = true,
                    CustomerId = order.CustomerId
                };

                await _notificationRepository.AddNotification(notification);

                // Mark the notification for the cancellation request as resolved
                var cancellationNotification = await _notificationRepository.GetNotificationByMessage($"You have received a cancellation request for the order {order.OrderId}");
                if (cancellationNotification != null)
                {
                    await _notificationRepository.MarkNotificationAsSeen(cancellationNotification.NotificationId);
                }
            }

            return updateResult;
        }

        // Method to reject a cancellation request
        public async Task<bool> RejectCancelOrder(Order order, string statusReason)
        {
            // Create a notification that cancellation is rejected
            var notification = new Notification
            {
                Message = $"Your order {order.OrderId} cannot be cancelled as it has already been {statusReason}.",
                IsVisibleToCSR = false,
                IsVisibleToAdmin = false,
                IsVisibleToVendor = false,
                IsVisibleToCustomer = true,
                IsResolved = true,
                CustomerId = order.CustomerId
            };

            await _notificationRepository.AddNotification(notification);

            order.OrderCancellation.CancelRequestStatus = CancelRequestStatus.Rejected;

            var orderUpdated = await _orderRepository.UpdateOrder(order);

            if (orderUpdated)
            {
                // Mark the notification for the cancellation request as resolved
                var cancellationNotification = await _notificationRepository.GetNotificationByMessage($"You have received a cancellation request for the order {order.OrderId}");
                if (cancellationNotification != null)
                {
                    await _notificationRepository.MarkNotificationAsSeen(cancellationNotification.NotificationId);
                }
            }

            return orderUpdated;
        }

        // Method to get order status
        public async Task<string> GetOrderStatus(string id)
        {
            return await _orderRepository.GetOrderStatus(id);
        }

        // Method to get pending cancellation requests
        public async Task<List<OrderDto>> GetPendingCancellationRequests()
        {
            var pendingOrders = await _orderRepository.GetOrdersByCancelRequestStatus(CancelRequestStatus.Pending);

            return pendingOrders.Select(order => new OrderDto
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
                            VendorId = oi.VendorId,
                            Quantity = oi.Quantity,
                            Price = oi.Price,
                            ProductStatus = oi.ProductStatus,
                        }).ToList()
                    }).ToList(),

                TotalAmount = order.TotalAmount,
                CustomerId = order.CustomerId,
                CreatedByCustomer = order.CreatedByCustomer,
                CreatedByAdmin = order.CreatedByAdmin,

                BillingDetails = order.BillingDetails != null ? new BillingDetailsDto
                {
                    CustomerName = order.BillingDetails.CustomerName,
                    Email = order.BillingDetails.Email,
                    Phone = order.BillingDetails.Phone,
                    SingleBillingAddress = order.BillingDetails.SingleBillingAddress,
                    BillingAddress = order.BillingDetails.BillingAddress != null ? new BillingAddressDto
                    {
                        StreetAddress = order.BillingDetails.BillingAddress.StreetAddress,
                        City = order.BillingDetails.BillingAddress.City,
                        Province = order.BillingDetails.BillingAddress.Province,
                        PostalCode = order.BillingDetails.BillingAddress.PostalCode,
                        Country = order.BillingDetails.BillingAddress.Country
                    } : null
                } : null,

                OrderCancellation = order.OrderCancellation != null ? new OrderCancellationDto
                {
                    Id = order.OrderCancellation.Id,
                    CancellationApproved = order.OrderCancellation.CancellationApproved,
                    CancellationDate = order.OrderCancellation.CancellationDate,
                    CancelRequestStatus = order.OrderCancellation.CancelRequestStatus,
                } : null

            }).ToList();
        }

        // Method to mark products as Delivered (Vendor-Specific)
        public async Task<bool> MarkProductAsDelivered(string orderId, string vendorId)
        {
            var order = await _orderRepository.GetOrderById(orderId);
            if (order == null || order.Status == OrderStatus.Delivered)
            {
                return false;
            }

            var vendorProducts = order.OrderItems.Where(item => item.VendorId == vendorId).ToList();

            if (!vendorProducts.Any())
            {
                return false;
            }

            foreach (var product in vendorProducts)
            {
                product.ProductStatus = ProductStatus.Delivered;
            }

            await _orderRepository.UpdateOrder(order);

            if (order.OrderItems.All(item => item.ProductStatus == ProductStatus.Delivered))
            {
                order.Status = OrderStatus.Delivered;

                // Create a notification for the customer
                var customerNotification = new Notification
                {
                    Message = $"Your order {order.OrderId} has been successfully delivered.",
                    IsVisibleToCSR = false,
                    IsVisibleToAdmin = false,
                    IsVisibleToVendor = false,
                    IsVisibleToCustomer = true,
                    IsResolved = true,
                    CustomerId = order.CustomerId
                };

                await _notificationRepository.AddNotification(customerNotification);
            }
            else
            {
                order.Status = OrderStatus.PartiallyDelivered;

                var productIds = string.Join(", ", vendorProducts.Select(p => p.ProductId));

                var partialNotificationMessage = $"The delivery for order {order.OrderId} is ready. The following products have been delivered: {productIds}.";

                var adminNotification = new Notification
                {
                    Message = partialNotificationMessage,
                    IsVisibleToCSR = true,
                    IsVisibleToAdmin = true,
                    IsVisibleToVendor = false,
                    IsVisibleToCustomer = false,
                    IsResolved = false,
                    CustomerId = null
                };

                await _notificationRepository.AddNotification(adminNotification);
            }

            await _orderRepository.UpdateOrder(order);

            return true;
        }

        // Method for CSR/Admin to mark entire order as Delivered
        public async Task<bool> MarkOrderAsDeliveredByAdmin(string orderId)
        {
            var order = await _orderRepository.GetOrderById(orderId);
            if (order == null || order.Status == OrderStatus.Delivered)
            {
                return false;
            }

            foreach (var product in order.OrderItems)
            {
                product.ProductStatus = ProductStatus.Delivered;
            }

            order.Status = OrderStatus.Delivered;

            await _orderRepository.UpdateOrder(order);
            return true;
        }

        // Get order items by vendorId (Vendor-Specific)
        public async Task<OrderDto> GetOrderByVendorId(string orderId, string vendorId)
        {
            var order = await _orderRepository.GetOrderByVendorId(orderId, vendorId);
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
                PaymentMethod = order.PaymentMethod ?? PaymentMethods.Visa,
                BillingDetails = order.BillingDetails != null
                    ? new BillingDetailsDto
                    {
                        CustomerName = order.BillingDetails.CustomerName,
                        Email = order.BillingDetails.Email,
                        Phone = order.BillingDetails.Phone,
                        SingleBillingAddress = order.BillingDetails.SingleBillingAddress,
                        BillingAddress = order.BillingDetails.BillingAddress != null
                            ? new BillingAddressDto
                            {
                                StreetAddress = order.BillingDetails.BillingAddress.StreetAddress,
                                City = order.BillingDetails.BillingAddress.City,
                                Province = order.BillingDetails.BillingAddress.Province,
                                PostalCode = order.BillingDetails.BillingAddress.PostalCode,
                                Country = order.BillingDetails.BillingAddress.Country
                            }
                            : null
                    }
                    : null,
                TotalAmount = order.TotalAmount,
                CreatedByCustomer = order.CreatedByCustomer,
                CreatedByAdmin = order.CreatedByAdmin,
                OrderItemsGroups = order.OrderItems
                    .GroupBy(oi => oi.ListItemId)
                    .Select(group => new OrderItemGroupDto
                    {
                        ListItemId = group.Key,
                        Items = group.Select(oi => new OrderItemDto
                        {
                            ProductId = oi.ProductId,
                            ProductName = oi.ProductName,
                            VendorId = oi.VendorId,
                            Quantity = oi.Quantity,
                            Price = oi.Price,
                            ProductStatus = oi.ProductStatus,
                        }).ToList()
                    }).ToList()
            };
        }

        // Get all orders related to the vendor
        public async Task<List<OrderDto>> GetOrdersByVendorId(string vendorId)
        {
            var orders = await _orderRepository.GetOrdersByVendorId(vendorId);
            if (orders == null || !orders.Any())
            {
                return new List<OrderDto>();
            }

            return orders.Select(order => new OrderDto
            {
                Id = order.Id,
                OrderId = order.OrderId,
                Status = order.Status,
                OrderDate = order.OrderDate,
                PaymentMethod = order.PaymentMethod ?? PaymentMethods.Visa,
                BillingDetails = order.BillingDetails != null
                    ? new BillingDetailsDto
                    {
                        CustomerName = order.BillingDetails.CustomerName,
                        Email = order.BillingDetails.Email,
                        Phone = order.BillingDetails.Phone,
                        SingleBillingAddress = order.BillingDetails.SingleBillingAddress,
                        BillingAddress = order.BillingDetails.BillingAddress != null
                            ? new BillingAddressDto
                            {
                                StreetAddress = order.BillingDetails.BillingAddress.StreetAddress,
                                City = order.BillingDetails.BillingAddress.City,
                                Province = order.BillingDetails.BillingAddress.Province,
                                PostalCode = order.BillingDetails.BillingAddress.PostalCode,
                                Country = order.BillingDetails.BillingAddress.Country
                            }
                            : null
                    }
                    : null,
                TotalAmount = order.TotalAmount,
                CreatedByCustomer = order.CreatedByCustomer,
                CreatedByAdmin = order.CreatedByAdmin,
                OrderItemsGroups = order.OrderItems
                    .Where(oi => oi.VendorId == vendorId)
                    .GroupBy(oi => oi.ListItemId)
                    .Select(group => new OrderItemGroupDto
                    {
                        ListItemId = group.Key,
                        Items = group.Select(oi => new OrderItemDto
                        {
                            ProductId = oi.ProductId,
                            ProductName = oi.ProductName,
                            VendorId = oi.VendorId,
                            Quantity = oi.Quantity,
                            Price = oi.Price,
                            ProductStatus = oi.ProductStatus,
                        }).ToList()
                    }).ToList()
            }).ToList();
        }
    }
}
