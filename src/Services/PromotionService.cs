using Ciandt.Retail.MCP.Interfaces.Services;
using Ciandt.Retail.MCP.Models.Result;

namespace Ciandt.Retail.MCP.Services;

public class PromotionService : IPromotionService
{
    public PromotionService()
    {
        
    }

    public Task<PromotionResult> GetActivePromotionsAsync(string categoryId)
    {
        throw new NotImplementedException();
    }
}
