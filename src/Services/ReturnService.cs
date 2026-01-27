using Ciandt.Retail.MCP.Models;

namespace Ciandt.Retail.MCP.Services;

public class ReturnService
{
    public async Task<ReturnRequest> InitiateReturnAsync(int orderId, List<ReturnItem> items, string returnReason)
    {
        throw new NotImplementedException();
    }

    public async Task<ReturnLabel> GenerateReturnLabelAsync(string returnId)
    {
        throw new NotImplementedException();
    }

    public async Task<ReturnStatus> GetReturnStatusAsync(string returnId)
    {
        throw new NotImplementedException();
    }

    public async Task<RefundStatus> CheckRefundStatusAsync(string refundId)
    {
        throw new NotImplementedException();
    }
}
