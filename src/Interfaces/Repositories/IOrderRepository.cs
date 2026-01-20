using Ciandt.Retail.MCP.Models;
using Ciandt.Retail.MCP.Models.Entities;
using Ciandt.Retail.MCP.Models.Result;

namespace Ciandt.Retail.MCP.Interfaces.Repositories;

public interface IOrderRepository
{
    Task<string> CreateOrderAsync(int customerId, ICollection<CartItem> items, decimal shippingCost, string? shippingMethod = null, string? shippingZipCode = null);
    Task<OrderEntity?> GetOrderByIdAsync(string orderId);
    Task<ICollection<OrderEntity>> GetOrdersByCustomerIdAsync(int customerId, int page = 1, int pageSize = 10);
    Task<bool> UpdateOrderStatusAsync(string orderId, OrderStatusEnum newStatus);
    Task<bool> UpdateTrackingNumberAsync(string orderId, string trackingNumber);
    Task<OrderTrackingResult> GetOrderTrackingAsync(string orderId);
    Task<OrderSummary> GetOrderSummaryAsync(string orderId);
    Task<bool> CancelOrderAsync(string orderId, string reason);
}