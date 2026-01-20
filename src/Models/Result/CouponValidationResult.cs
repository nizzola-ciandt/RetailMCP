namespace Ciandt.Retail.MCP.Models.Result;

public class CouponValidationResult
{
    public bool IsValid { get; set; }
    public string Message { get; set; } = string.Empty;
    public string CouponCode { get; set; } = string.Empty;
    public decimal DiscountAmount { get; set; }
    public decimal? MinimumPurchaseAmount { get; set; }
    public decimal FinalAmount { get; set; }
    public string? DiscountType { get; set; }
    public string? DiscountPercentage { get; set; }
}
