namespace Ciandt.Retail.MCP.Models;

public class PaymentProcessingResult
{
    public bool Success { get; set; }
    public string PaymentId { get; set; } = string.Empty;
    public string PaymentUrl { get; set; } = string.Empty;
    public string PaymentStatus { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? PaymentDetails { get; set; }
}