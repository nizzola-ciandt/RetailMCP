using Ciandt.Retail.MCP.Data;
using Ciandt.Retail.MCP.Interfaces.Repositories;
using Ciandt.Retail.MCP.Models;
using Ciandt.Retail.MCP.Models.Entities;
using Ciandt.Retail.MCP.Models.ModelExtensions;
using Ciandt.Retail.MCP.Models.Result;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Ciandt.Retail.MCP.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly RetailDbContext _context;
    private readonly ILogger<ProductRepository> _logger;

    public ProductRepository(RetailDbContext context, ILogger<ProductRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ICollection<ProductSummary>> GetByCriteria(ProductSearchCriteria criteria)
    {
        try
        {
            IQueryable<ProductEntity> query = _context.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(criteria.Name))
            {
                query = query.Where(p => EF.Functions.Like(p.Name, $"%{criteria.Name}%"));
            }

            if (!string.IsNullOrWhiteSpace(criteria.Category))
            {
                query = query.Where(p => EF.Functions.Like(p.Category, $"%{criteria.Category}%"));
            }

            if (criteria.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= criteria.MinPrice.Value);
            }

            if (criteria.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= criteria.MaxPrice.Value);
            }

            var products = await query.ToListAsync();

            return products.Select(p => new ProductSummary
            {
                ProductId = p.ProductId,
                Name = p.Name,
                Brand = p.Brand,
                Category = p.Category,
                Price = p.Price,
                DiscountedPrice = p.DiscountedPrice,
                ImageUrl = p.ImageUrl ?? string.Empty,
                AverageRating = p.AverageRating,
                ReviewCount = p.ReviewCount,
                InStock = p.InStock,
                AvailableColors = DeserializeList(p.AvailableColors),
                AvailableSizes = DeserializeList(p.AvailableSizes),
                HasPromotion = p.HasPromotion,
                PromoLabel = p.PromoLabel ?? string.Empty,
                IsBestSeller = p.IsBestSeller,
                IsNew = p.IsNew
            }).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting products by criteria");
            return new List<ProductSummary>();
        }
    }

    public async Task<ProductDetailResult> GetProductDetailsAsync(string productId)
    {
        try
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductId == productId);

            if (product == null)
            {
                _logger.LogWarning($"Product not found: {productId}");
                return null!;
            }

            var productSummary = new ProductSummary
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Brand = product.Brand,
                Category = product.Category,
                Price = product.Price,
                DiscountedPrice = product.DiscountedPrice,
                ImageUrl = product.ImageUrl ?? string.Empty,
                AverageRating = product.AverageRating,
                ReviewCount = product.ReviewCount,
                InStock = product.InStock,
                AvailableColors = DeserializeList(product.AvailableColors),
                AvailableSizes = DeserializeList(product.AvailableSizes),
                HasPromotion = product.HasPromotion,
                PromoLabel = product.PromoLabel ?? string.Empty,
                IsBestSeller = product.IsBestSeller,
                IsNew = product.IsNew
            };

            return productSummary.ToProductDetailResult();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting product details: {productId}");
            return null!;
        }
    }

    public async Task<CategorySearchResult> GetCategories()
    {
        try
        {
            var categories = await _context.Products
                .Select(p => p.Category)
                .Distinct()
                .ToListAsync();

            return new CategorySearchResult { CategoryNames = categories };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting categories");
            return new CategorySearchResult { CategoryNames = new List<string>() };
        }
    }

    public Task<ProductAvailability> CheckProductAvailabilityAsync(string productId, string location = null)
    {
        throw new NotImplementedException();
    }

    public Task<List<ProductReview>> GetProductReviewsAsync(string productId, int limit = 5)
    {
        throw new NotImplementedException();
    }

    private List<string> DeserializeList(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return new List<string>();

        try
        {
            return JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();
        }
        catch
        {
            return new List<string>();
        }
    }
}