namespace Ciandt.Retail.MCP.Models.Result;

public class OrderListResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<OrderSummary> Orders { get; set; } = new();
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
}