using Ciandt.Retail.MCP.Models.Entities;

namespace Ciandt.Retail.MCP.Models.Result;

public class RefundStatusResult
{
    public string RefundId { get; set; }
    public int OrderId { get; set; }
    public string RefundStatus { get; set; }
    public decimal RefundAmount { get; set; }
    public string RefundMethod { get; set; }
    public DateTime RequestDate { get; set; }
    public DateTime? ProcessedDate { get; set; }
    public string PaymentAccount { get; set; } // Last 4 digits or token
    public int? EstimatedDaysToComplete { get; set; }
    public List<RefundItem> Items { get; set; } = new List<RefundItem>();
    public OrderStatusEnum Status { get; set;  }
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime ExpectedCompletionDate { get; set; }
}

public class RefundItem
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal RefundAmount { get; set; }
    public string RefundReason { get; set; }
}