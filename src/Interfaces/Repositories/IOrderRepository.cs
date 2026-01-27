using Ciandt.Retail.MCP.Models;
using Ciandt.Retail.MCP.Models.Entities;
using Ciandt.Retail.MCP.Models.Result;

namespace Ciandt.Retail.MCP.Interfaces.Repositories;

public interface IOrderRepository
{
    Task<int> CreateOrderAsync(int customerId, ICollection<CartItem> items, decimal shippingCost, string? shippingMethod = null, string? shippingZipCode = null);
    Task<OrderEntity?> GetOrderByIdAsync(int orderId);
    Task<ICollection<OrderEntity>> GetOrdersByCustomerIdAsync(int customerId, int page = 1, int pageSize = 10);
    Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatusEnum newStatus);
    Task<bool> UpdateTrackingNumberAsync(int orderId, string trackingNumber);
    Task<OrderTrackingResult> GetOrderTrackingAsync(int orderId);
    Task<OrderSummary> GetOrderSummaryAsync(int orderId);
    Task<bool> CancelOrderAsync(int orderId, string reason);
}