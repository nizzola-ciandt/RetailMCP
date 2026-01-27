using Ciandt.Retail.MCP.Interfaces.Repositories;
using Ciandt.Retail.MCP.Interfaces.Services;
using Ciandt.Retail.MCP.Models;
using Ciandt.Retail.MCP.Models.Result;

namespace Ciandt.Retail.MCP.Services;

public class ProductsService : IProductsService
{
    public readonly IProductRepository _productRepository;
    public readonly ILogger _logger;
    public readonly string _productImageUrl;
    public ProductsService(IProductRepository productRepository, ILogger<ProductsService> logger, IConfiguration config)
    {
        _productRepository = productRepository;
        _logger = logger;
        _productImageUrl = config.GetSection("ProductImageUrl").Value;
    }
    public async Task<ProductSearchResult> SearchProductsAsync(ProductSearchCriteria criteria, int page = 1, int limit = 10)
    {
        var response = new ProductSearchResult();
        response.Products = await _productRepository.GetByCriteria(criteria);
        response.CurrentPage = 1;
        response.TotalPages = 1;
        response.TotalCount = response.Products.Count();

        PrepareImageUrl(response.Products);

        return response;
    }

    private void PrepareImageUrl(ICollection<ProductSummary> products)
    {
        foreach (var prod in products)
        {
            prod.ImageUrl = _productImageUrl + prod.ImageUrl;
        }
    }

    public async Task<ProductDetailResult> GetProductDetailsAsync(string productId)
    {
        return await _productRepository.GetProductDetailsAsync(productId);
    }

    public async Task<ProductAvailability> CheckProductAvailabilityAsync(string productId, string location = null)
    {
        throw new NotImplementedException();
    }

    public async Task<ProductComparisonResult> CompareProductsAsync(List<string> productIds)
    {
        throw new NotImplementedException();
    }

    public async Task<List<ProductReview>> GetProductReviewsAsync(string productId, int limit = 5)
    {
        throw new NotImplementedException();
    }

    public Task<CategorySearchResult> ListCategoriesAsync()
    {
        return _productRepository.GetCategories();
    }
}
