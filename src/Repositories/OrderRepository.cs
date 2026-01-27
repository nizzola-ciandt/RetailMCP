using Ciandt.Retail.MCP.Data;
using Ciandt.Retail.MCP.Interfaces.Repositories;
using Ciandt.Retail.MCP.Models;
using Ciandt.Retail.MCP.Models.Entities;
using Ciandt.Retail.MCP.Models.Result;
using Microsoft.EntityFrameworkCore;

namespace Ciandt.Retail.MCP.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly RetailDbContext _context;
    private readonly ILogger<OrderRepository> _logger;

    public OrderRepository(RetailDbContext context, ILogger<OrderRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<int> CreateOrderAsync(int customerId, ICollection<CartItem> items, decimal shippingCost, string? shippingMethod = null, string? shippingZipCode = null)
    {
        try
        {
            var orderItems = items.Select(item => new OrderItemEntity
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                Quantity = item.Quantity,
                UnitPrice = item.Price,
                TotalPrice = item.Price * item.Quantity
            }).ToList();

            var totalAmount = orderItems.Sum(i => i.TotalPrice) + shippingCost;

            var order = new OrderEntity
            {
             //   OrderId = orderId,
                CustomerId = customerId,
                TotalAmount = totalAmount,
                ShippingCost = shippingCost,
                DiscountAmount = 0,
                Status = OrderStatusEnum.Pending,
                ShippingMethod = shippingMethod,
                ShippingZipCode = shippingZipCode,
                CreatedAt = DateTime.UtcNow,
                OrderItems = orderItems
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Order created successfully: {order.Id}");
            return order.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating order");
            throw;
        }
    }

    public async Task<OrderEntity?> GetOrderByIdAsync(int orderId)
    {
        try
        {
            return await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .Include(o => o.Payments)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting order: {orderId}");
            return null;
        }
    }

    public async Task<ICollection<OrderEntity>> GetOrdersByCustomerIdAsync(int customerId, int page = 1, int pageSize = 10)
    {
        try
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.Payments)
                .Where(o => o.CustomerId == customerId)
                .OrderByDescending(o => o.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting orders for customer: {customerId}");
            return new List<OrderEntity>();
        }
    }

    public async Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatusEnum newStatus)
    {
        try
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                _logger.LogWarning($"Order not found: {orderId}");
                return false;
            }

            order.Status = newStatus;

            switch (newStatus)
            {
                case OrderStatusEnum.Shipped:
                    order.ShippedAt = DateTime.UtcNow;
                    break;
                case OrderStatusEnum.Delivered:
                    order.DeliveredAt = DateTime.UtcNow;
                    break;
                case OrderStatusEnum.Cancelled:
                    order.CancelledAt = DateTime.UtcNow;
                    break;
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation($"Order status updated: {orderId} -> {newStatus}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating order status: {orderId}");
            return false;
        }
    }

    public async Task<bool> UpdateTrackingNumberAsync(int orderId, string trackingNumber)
    {
        try
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                _logger.LogWarning($"Order not found: {orderId}");
                return false;
            }

            order.TrackingNumber = trackingNumber;
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Tracking number updated for order: {orderId}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating tracking number: {orderId}");
            return false;
        }
    }

    public async Task<OrderTrackingResult> GetOrderTrackingAsync(int orderId)
    {
        try
        {
            var order = await GetOrderByIdAsync(orderId);
            if (order == null)
            {
                return new OrderTrackingResult
                {
                    Success = false,
                    Message = "Order not found"
                };
            }

            return new OrderTrackingResult
            {
                Success = true,
                OrderId = order.Id,
                Status = order.Status,
                TrackingNumber = order.TrackingNumber,
                ShippingMethod = order.ShippingMethod,
                EstimatedDeliveryDate = order.ShippedAt?.AddDays(7),
                ActualDeliveryDate = order.DeliveredAt,
                Message = GetTrackingMessage(order.Status)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting order tracking: {orderId}");
            return new OrderTrackingResult
            {
                Success = false,
                Message = "Error retrieving tracking information"
            };
        }
    }

    public async Task<OrderSummary> GetOrderSummaryAsync(int orderId)
    {
        try
        {
            var order = await GetOrderByIdAsync(orderId);
            if (order == null)
            {
                return new OrderSummary();
            }

            return new OrderSummary
            {
                OrderId = order.Id,
                CustomerId = order.CustomerId,
                CustomerName = order.Customer.Name,
                TotalAmount = order.TotalAmount,
                ShippingCost = order.ShippingCost,
                DiscountAmount = order.DiscountAmount,
                Status = order.Status,
                TrackingNumber = order.TrackingNumber,
                ShippingMethod = order.ShippingMethod,
                CreatedAt = order.CreatedAt,
                ShippedAt = order.ShippedAt,
                DeliveredAt = order.DeliveredAt,
                Items = order.OrderItems.Select(i => new OrderItemSummary
                {
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    TotalPrice = i.TotalPrice
                }).ToList(),
                Payments = order.Payments.Select(p => new PaymentSummary
                {
                    PaymentId = p.PaymentId,
                    PaymentMethodId = p.PaymentMethodId,
                    Amount = p.Amount,
                    Installments = p.Installments,
                    Status = p.Status,
                    CreatedAt = p.CreatedAt
                }).ToList()
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting order summary: {orderId}");
            return new OrderSummary();
        }
    }

    public async Task<bool> CancelOrderAsync(int orderId, string reason)
    {
        try
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                _logger.LogWarning($"Order not found: {orderId}");
                return false;
            }

            if (order.Status == OrderStatusEnum.Delivered || order.Status == OrderStatusEnum.Cancelled)
            {
                _logger.LogWarning($"Cannot cancel order in status: {order.Status}");
                return false;
            }

            order.Status = OrderStatusEnum.Cancelled;
            order.CancelledAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            _logger.LogInformation($"Order cancelled: {orderId}, Reason: {reason}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error cancelling order: {orderId}");
            return false;
        }
    }

    private string GetTrackingMessage(OrderStatusEnum status)
    {
        return status switch
        {
            OrderStatusEnum.Pending => "Seu pedido foi recebido e está sendo processado.",
            OrderStatusEnum.Processing => "Seu pedido está sendo preparado para envio.",
            OrderStatusEnum.Shipped => "Seu pedido foi enviado e está a caminho.",
            OrderStatusEnum.Delivered => "Seu pedido foi entregue com sucesso.",
            OrderStatusEnum.Cancelled => "Seu pedido foi cancelado.",
            _ => "Status desconhecido."
        };
    }
}