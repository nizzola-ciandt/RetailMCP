namespace Ciandt.Retail.MCP.Models.Result;

public class ShippingOptionsResult
{
    public ICollection<ShippingOption> Options { get; set; } = new List<ShippingOption>();
    public string EstimatedDeliveryTimeFrame { get; set; }
    public string ZipCode { get; set; }
}

