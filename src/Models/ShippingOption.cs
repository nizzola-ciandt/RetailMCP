namespace Ciandt.Retail.MCP.Models;

public class ShippingOption
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Carrier { get; set; } = string.Empty;
    public decimal Cost { get; set; }
    public int EstimatedDeliveryDays { get; set; }
    public bool IsExpedited { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsAvailable { get; set; } = true;
}