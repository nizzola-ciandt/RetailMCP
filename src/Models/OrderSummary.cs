using Ciandt.Retail.MCP.Models.Entities;

namespace Ciandt.Retail.MCP.Models;

public class OrderSummary
{
    public string OrderId { get; set; } = string.Empty;
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

public class OrderItemSummary
{
    public string ProductId { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
}

public class PaymentSummary
{
    public string PaymentId { get; set; } = string.Empty;
    public string PaymentMethodId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public int Installments { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}