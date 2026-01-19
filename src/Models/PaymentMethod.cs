namespace Ciandt.Retail.MCP.Models;

public class PaymentMethod
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; } // Credit, Debit, PayPal, etc.
    public string Description { get; set; }
    public bool SupportsInstallments { get; set; }
    public int? MaxInstallments { get; set; }
    public decimal? HandlingFee { get; set; }
    public string IconUrl { get; set; }
}
