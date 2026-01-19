namespace Ciandt.Retail.MCP.Models.Result;

public class CategorySearchResult
{
    public ICollection<string> CategoryNames { get; set; } = new List<string>();
}
