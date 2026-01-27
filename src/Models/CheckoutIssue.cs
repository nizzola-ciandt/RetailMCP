namespace Ciandt.Retail.MCP.Models;

// Class representing an issue with checkout
public class CheckoutIssue
{
    public int ProductId { get; set; }
    public string IssueType { get; set; } // "OutOfStock", "QuantityLimited", "PriceChanged"
    public string Description { get; set; }
    public bool IsBlocking { get; set; }
}
