using Ciandt.Retail.MCP.Models;
using Ciandt.Retail.MCP.Models.Result;

namespace Ciandt.Retail.MCP.Interfaces.Services;
public interface IProductsService
{
    Task<ProductSearchResult> SearchProductsAsync(ProductSearchCriteria criteria, int page = 1, int limit = 10);
    Task<ProductComparisonResult> CompareProductsAsync(List<string> productIds);
    Task<ProductDetailResult> GetProductDetailsAsync(string productId);
    Task<CategorySearchResult> ListCategoriesAsync();
    //Task<List<ProductReview>> GetProductReviewsAsync(string productId, int limit = 5);
    //Task<ProductAvailability> CheckProductAvailabilityAsync(string productId, string location = "");
}