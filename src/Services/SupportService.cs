using Ciandt.Retail.MCP.Models;

namespace Ciandt.Retail.MCP.Services;

public class SupportService
{
    public async Task<List<KnowledgeBaseArticle>> SearchKnowledgeBaseAsync(string query, string category = null)
    {
        // Implementação para buscar no FAQ
        throw new NotImplementedException();
    }

    public async Task<SupportTicket> ReportIssueAsync(string customerId, string orderId, string issueType, string description)
    {
        // Implementação para registrar problema em sistema automatizado (Jira/ServiceNow/Etc)
        throw new NotImplementedException();
    }

    public async Task<EscalationResult> EscalateToHumanAsync(string conversationId, ConversationContext context, string reason)
    {
        // Implementação para escalar atendimento
        throw new NotImplementedException();
    }
}
