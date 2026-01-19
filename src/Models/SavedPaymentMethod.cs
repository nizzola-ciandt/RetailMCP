namespace Ciandt.Retail.MCP.Models;

public class SavedPaymentMethod
{
    public string Id { get; set; }
    public string Type { get; set; }
    public string DisplayName { get; set; }
    public string LastFourDigits { get; set; }
    public string CardBrand { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public bool IsDefault { get; set; }
}
