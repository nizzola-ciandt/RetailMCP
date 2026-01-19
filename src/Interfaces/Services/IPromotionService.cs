using Ciandt.Retail.MCP.Models.Result;

namespace Ciandt.Retail.MCP.Interfaces.Services;

public interface IPromotionService
{
    Task<PromotionResult> GetActivePromotionsAsync(string categoryId);
}