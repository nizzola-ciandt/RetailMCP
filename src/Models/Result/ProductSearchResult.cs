namespace Ciandt.Retail.MCP.Models.Result;

public class ProductSearchResult
{
    public ICollection<ProductSummary> Products { get; set; } = new List<ProductSummary>();
    public int TotalCount { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public ProductSearchCriteria AppliedCriteria { get; set; } = new ProductSearchCriteria();
}
