namespace Ciandt.Retail.MCP.Models.Result;

public class ReturnRequestResult
{
    public string ReturnId { get; set; }
    public string OrderId { get; set; }
    public List<ReturnItem> Items { get; set; } = new List<ReturnItem>();
    public string ReturnStatus { get; set; }
    public DateTime RequestDate { get; set; }
    public string ReturnMethod { get; set; }
    public string ReturnLabel { get; set; } // URL or base64 encoded label
    public DateTime? ReturnDeadline { get; set; }
    public string ReturnInstructions { get; set; }
}

public class ReturnItem
{
    public string ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public string ReturnReason { get; set; }
    public decimal RefundAmount { get; set; }
}