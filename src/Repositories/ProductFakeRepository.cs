using Ciandt.Retail.MCP.Interfaces.Repositories;
using Ciandt.Retail.MCP.Models;
using Ciandt.Retail.MCP.Models.ModelExtensions;
using Ciandt.Retail.MCP.Models.Result;
using Ciandt.Retail.MCP.Services;

namespace Ciandt.Retail.MCP.Repository;

public class ProductFakeRepository : IProductRepository
{
    private readonly ICollection<ProductSummary> _products = new List<ProductSummary>();
    public ProductFakeRepository()
    {
        _products = GenerateFakeProduct();
    }

    public async Task<ICollection<ProductSummary>> GetByCriteria(ProductSearchCriteria criteria)
    {
        // Simular uma pesquisa assíncrona no repositório fake
        IQueryable<ProductSummary> query = _products.AsQueryable();

        // Aplicar os critérios de pesquisa
        if (!string.IsNullOrWhiteSpace(criteria.Name))
        {
            //query = query.Where(p => p.Name.Contains(criteria.Name));
            query = query.Where(p => p.Name.IndexOf(criteria.Name, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        if (!string.IsNullOrWhiteSpace(criteria.Category))
        {
            //query = query.Where(p => p.Name.Contains(criteria.Name));
            query = query.Where(p => p.Name.IndexOf(criteria.Category, StringComparison.OrdinalIgnoreCase) >= 0);
            query = query.Where(p => p.Category.IndexOf(criteria.Category, StringComparison.OrdinalIgnoreCase) >= 0);
        }


        if (criteria.MinPrice.HasValue)
        {
            query = query.Where(p => p.Price >= criteria.MinPrice.Value);
        }

        if (criteria.MaxPrice.HasValue)
        {
            query = query.Where(p => p.Price <= criteria.MaxPrice.Value);
        }

        return query.ToList();
    }

    public async Task<ProductDetailResult> GetProductDetailsAsync(string productId)
    {
        var product = _products.Where(a => a.ProductId == productId).FirstOrDefault();
        if (product != null)
        {
            return product.ToProductDetailResult();
        }
        return null;
    }

    
    public async Task<ProductAvailability> CheckProductAvailabilityAsync(string productId, string location = null)
    {
        throw new NotImplementedException();
    }

    public async Task<List<ProductReview>> GetProductReviewsAsync(string productId, int limit = 5)
    {
        throw new NotImplementedException();
    }

    public async Task<CategorySearchResult> GetCategories()
    {
        var categories = _products.Select(a => a.Category).Distinct().ToList();
        var response = new CategorySearchResult() { CategoryNames = categories };

        return response;
    }

    public ICollection<ProductSummary> GenerateFakeProduct()
    {
        return new List<ProductSummary>
        {
            // Roteador 1
            new ProductSummary
            {
                ProductId = "ROT-001",
                Name = "Roteador NetSpeed 500 Dual Band",
                Brand = "TechConnect",
                Category = "Roteadores",
                Price = 299.99m,
                DiscountedPrice = 249.99m,
                ImageUrl = "https://example.com/images/router-netspeed-500.jpg",
                AverageRating = 4.7,
                ReviewCount = 1258,
                InStock = true,
                AvailableColors = new List<string> { "Preto", "Branco" },
                AvailableSizes = null,
                HasPromotion = true,
                PromoLabel = "Oferta da Semana",
                IsBestSeller = true,
                IsNew = false
            },

            // Roteador 2
            new ProductSummary
            {
                ProductId = "ROT-002",
                Name = "Roteador UltraConnect WiFi 6 Mesh",
                Brand = "NetMaster",
                Category = "Roteadores",
                Price = 599.99m,
                DiscountedPrice = null,
                ImageUrl = "https://example.com/images/router-ultraconnect-wifi6.jpg",
                AverageRating = 4.9,
                ReviewCount = 358,
                InStock = true,
                AvailableColors = new List<string> { "Preto" },
                AvailableSizes = null,
                HasPromotion = false,
                PromoLabel = null,
                IsBestSeller = false,
                IsNew = true
            },

            // Impressora
            new ProductSummary
            {
                ProductId = "IMP-001",
                Name = "Impressora ColorJet Pro Multifuncional",
                Brand = "PrintMaster",
                Category = "Impressoras",
                Price = 899.99m,
                DiscountedPrice = 799.99m,
                ImageUrl = "https://example.com/images/printer-colorjet-pro.jpg",
                AverageRating = 4.3,
                ReviewCount = 562,
                InStock = true,
                AvailableColors = new List<string> { "Preto", "Cinza" },
                AvailableSizes = null,
                HasPromotion = true,
                PromoLabel = "Frete Grátis",
                IsBestSeller = false,
                IsNew = false
            },

            // Notebook
            new ProductSummary
            {
                ProductId = "NOT-001",
                Name = "Notebook UltraBook X5 Core i7",
                Brand = "TechPro",
                Category = "Notebooks",
                Price = 4599.99m,
                DiscountedPrice = 4299.99m,
                ImageUrl = "https://example.com/images/notebook-ultrabook-x5.jpg",
                AverageRating = 4.8,
                ReviewCount = 1879,
                InStock = true,
                AvailableColors = new List<string> { "Prata", "Grafite" },
                AvailableSizes = null,
                HasPromotion = true,
                PromoLabel = "Cashback 10%",
                IsBestSeller = true,
                IsNew = false
            },

            // Tablet
            new ProductSummary
            {
                ProductId = "TAB-001",
                Name = "Tablet TabX Pro 10.5\"",
                Brand = "GalaxyTech",
                Category = "Tablets",
                Price = 2199.99m,
                DiscountedPrice = null,
                ImageUrl = "https://example.com/images/tablet-tabx-pro.jpg",
                AverageRating = 4.5,
                ReviewCount = 892,
                InStock = false,  // Produto fora de estoque
                AvailableColors = new List<string> { "Preto", "Azul", "Rosa" },
                AvailableSizes = new List<string> { "64GB", "128GB", "256GB" },
                HasPromotion = false,
                PromoLabel = null,
                IsBestSeller = false,
                IsNew = true
            }
        };
    }
}
