namespace Ciandt.Retail.MCP.Models;

public class ShippingOption
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Carrier { get; set; }
    public decimal Cost { get; set; }
    public int EstimatedDeliveryDays { get; set; }
    public bool IsExpedited { get; set; }
    public string Description { get; set; }
}