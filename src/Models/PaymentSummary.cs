namespace Ciandt.Retail.MCP.Models;

public class PaymentSummary
{
    public string PaymentId { get; set; } = string.Empty;
    public string PaymentMethodId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public int Installments { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}