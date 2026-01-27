using Ciandt.Retail.MCP.Models;
using Ciandt.Retail.MCP.Models.Result;

namespace Ciandt.Retail.MCP.Interfaces.Services;
public interface IProductsService
{
    Task<ProductSearchResult> SearchProductsAsync(ProductSearchCriteria criteria, int page = 1, int limit = 10);
    Task<ProductComparisonResult> CompareProductsAsync(List<string> productIds);
    Task<ProductDetailResult> GetProductDetailsAsync(int ProductId);
    Task<CategorySearchResult> ListCategoriesAsync();
    //Task<List<ProductReview>> GetProductReviewsAsync(int ProductId, int limit = 5);
    //Task<ProductAvailability> CheckProductAvailabilityAsync(int ProductId, string location = "");
}