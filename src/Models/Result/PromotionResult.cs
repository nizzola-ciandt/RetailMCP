namespace Ciandt.Retail.MCP.Models.Result;

public class PromotionResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<PromotionInfo> Promotions { get; set; } = new();
}

public class PromotionInfo
{
    public string PromotionId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal DiscountPercentage { get; set; }
    public decimal? DiscountAmount { get; set; }
    public string? CategoryId { get; set; }
    public int? ProductId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}