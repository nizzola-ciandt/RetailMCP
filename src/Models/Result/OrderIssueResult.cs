namespace Ciandt.Retail.MCP.Models.Result;

public class OrderIssueResult
{
    public string IssueId { get; set; }
    public string OrderId { get; set; }
    public string IssueType { get; set; }
    public string IssueStatus { get; set; }
    public DateTime ReportedDate { get; set; }
    public string AgentAssigned { get; set; }
    public DateTime? ExpectedResolutionDate { get; set; }
    public List<IssueComment> Comments { get; set; } = new List<IssueComment>();
    public string NextSteps { get; set; }
}

public class IssueComment
{
    public string AuthorId { get; set; }
    public string AuthorName { get; set; }
    public string AuthorType { get; set; } // "Customer" or "Agent"
    public DateTime Timestamp { get; set; }
    public string Content { get; set; }
}
