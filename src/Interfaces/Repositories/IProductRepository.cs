using Ciandt.Retail.MCP.Models;
using Ciandt.Retail.MCP.Models.Result;

namespace Ciandt.Retail.MCP.Interfaces.Repositories;

public interface IProductRepository
{
    Task<ICollection<ProductSummary>> GetByCriteria( ProductSearchCriteria criteria );
    Task<ProductDetailResult> GetProductDetailsAsync(int productId);
    Task<ProductAvailability> CheckProductAvailabilityAsync(int productId, string location = null);
    Task<List<ProductReview>> GetProductReviewsAsync(int productId, int limit = 5);
    Task<CategorySearchResult> GetCategories();
}
