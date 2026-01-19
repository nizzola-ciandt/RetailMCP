namespace Ciandt.Retail.MCP.Models;

public class PaymentPromotion
{
    public string Id { get; set; }
    public string Description { get; set; }
    public string PaymentMethodId { get; set; }
    public decimal DiscountPercentage { get; set; }
    public int? Installments { get; set; }
    public bool IsInterestFree { get; set; }
}