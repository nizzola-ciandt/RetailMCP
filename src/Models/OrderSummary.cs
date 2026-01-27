using Ciandt.Retail.MCP.Models.Entities;

namespace Ciandt.Retail.MCP.Models;

public class OrderSummary
{
    public int OrderId { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public decimal ShippingCost { get; set; }
    public decimal DiscountAmount { get; set; }
    public OrderStatusEnum Status { get; set; } = OrderStatusEnum.Unknown;
    public string? TrackingNumber { get; set; }
    public string? ShippingMethod { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ShippedAt { get; set; }
    public DateTime? DeliveredAt { get; set; }
    public List<OrderItemSummary> Items { get; set; } = new();
    public List<PaymentSummary> Payments { get; set; } = new();
}
