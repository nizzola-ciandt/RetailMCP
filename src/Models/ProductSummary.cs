namespace Ciandt.Retail.MCP.Models;

public class ProductSummary
{
    public string ProductId { get; set; }
    public string Name { get; set; }
    public string Brand { get; set; }
    public string Category { get; set; }
    public decimal Price { get; set; }
    public decimal? DiscountedPrice { get; set; }
    public string ImageUrl { get; set; }
    public double AverageRating { get; set; }
    public int ReviewCount { get; set; }
    public bool InStock { get; set; }
    public List<string> AvailableColors { get; set; }
    public List<string> AvailableSizes { get; set; }
    public bool HasPromotion { get; set; }
    public string PromoLabel { get; set; }
    public bool IsBestSeller { get; set; }
    public bool IsNew { get; set; }
}
