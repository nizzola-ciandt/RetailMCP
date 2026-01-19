using Ciandt.Retail.MCP.Interfaces.Services;
using Ciandt.Retail.MCP.Models;
using Ciandt.Retail.MCP.Models.Result;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Text.Json;

namespace Ciandt.Retail.MCP.Tools;

[McpServerToolType]
public class ProductTool
{
    private readonly IProductsService _productsService;
    private readonly ILogger _logger;
    public ProductTool(IProductsService productsService, ILogger<ProductTool> logger)
    {
        _productsService = productsService;
        _logger = logger;
    }

    [McpServerTool(Name = "product_search", Title = "Customer searches for specific products or product categories in the catalog")]
    [Description("Analyzes the customer query request and provides a response with matching products from catalog.")]
    public async Task<ProductSearchResult> SearchProductAvailabilityQueryAsync(ProductSearchCriteria criteria)
    {
        _logger.LogInformation($"SearchProductAvailabilityQueryAsync - Received request: {JsonSerializer.Serialize(criteria)}");
        return await _productsService.SearchProductsAsync(criteria, 1, 10);
    }

    [McpServerTool(Name = "productcategory_search", Title = "List all categories of products from inventory")]
    [Description("List all categories of products from inventory")]
    public async Task<CategorySearchResult> SearchCategoryAvailabilityQueryAsync()
    {
        return await _productsService.ListCategoriesAsync( );
    }

    [McpServerTool(Name = "product_inquiry", Title = "Customer requests detailed information about product features or specifications")]
    [Description("Retrieves detailed product information including specifications, features, and availability.")]
    public async Task<ProductDetailResult> GetProductDetailsAsync(string productId)
    {
        _logger.LogInformation($"Retrieving details for product ID: {productId}");
        return await _productsService.GetProductDetailsAsync(productId);
    }
    /*
    [McpServerTool(Name = "price_comparison", Title = "Customer wants to compare different products or similar models to understand differences")]
    [Description("Compares details between multiple products and returns a comparison table.")]
    public async Task<ProductComparisonResult> CompareProductAsync(List<string> productIds)
    {
        _logger.LogInformation($"Comparing prices for {productIds.Count} products");
        return await _productsService.CompareProductsAsync(productIds);
    }
    */
}
