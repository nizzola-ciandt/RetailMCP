using Ciandt.Retail.MCP.Models.Entities;

namespace Ciandt.Retail.MCP.Models.Result;

public class OrderTrackingResult
{
    public int OrderId { get; set; }
    public OrderStatusEnum Status { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal OrderTotal { get; set; }
    public List<OrderTrackingEvent> TrackingEvents { get; set; } = new List<OrderTrackingEvent>();
    public string TrackingNumber { get; set; }
    public string CarrierName { get; set; }
    public DateTime? EstimatedDeliveryDate { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; }
    public string? ShippingMethod { get; set; }
    public DateTime? ActualDeliveryDate { get; set; }
}

public class OrderTrackingEvent
{
    public DateTime Timestamp { get; set; }
    public string Status { get; set; }
    public string Location { get; set; }
    public string Description { get; set; }
}
