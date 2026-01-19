namespace Ciandt.Retail.MCP.Models;

public class ConversationContext
{
    public string ConversationId { get; set; }
    public string CustomerId { get; set; }
    public string CurrentIntent { get; set; }
    public List<string> RecentMessages { get; set; }
    public Dictionary<string, object> SessionState { get; set; }
}