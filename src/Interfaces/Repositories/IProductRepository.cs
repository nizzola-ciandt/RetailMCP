using Ciandt.Retail.MCP.Models;
using Ciandt.Retail.MCP.Models.Result;

namespace Ciandt.Retail.MCP.Interfaces.Repositories;

public interface IProductRepository
{
    Task<ICollection<ProductSummary>> GetByCriteria( ProductSearchCriteria criteria );
    Task<ProductDetailResult> GetProductDetailsAsync(string productId);
    Task<ProductAvailability> CheckProductAvailabilityAsync(string productId, string location = null);
    Task<List<ProductReview>> GetProductReviewsAsync(string productId, int limit = 5);
    Task<CategorySearchResult> GetCategories();
}
