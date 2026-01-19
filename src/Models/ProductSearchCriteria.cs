namespace Ciandt.Retail.MCP.Models;

public class ProductSearchCriteria
{
    public string Name { get; set; }
    public string Category { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    //public List<string> Brands { get; set; }
    //public Dictionary<string, List<string>> Attributes { get; set; }
    public string SortBy { get; set; }
}
