namespace Ciandt.Retail.MCP.Models.Result;

public class OrderTrackingResult
{
    public string OrderId { get; set; }
    public string OrderStatus { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal OrderTotal { get; set; }
    public List<OrderTrackingEvent> TrackingEvents { get; set; } = new List<OrderTrackingEvent>();
    public string TrackingNumber { get; set; }
    public string CarrierName { get; set; }
    public string EstimatedDeliveryDate { get; set; }
}

public class OrderTrackingEvent
{
    public DateTime Timestamp { get; set; }
    public string Status { get; set; }
    public string Location { get; set; }
    public string Description { get; set; }
}
