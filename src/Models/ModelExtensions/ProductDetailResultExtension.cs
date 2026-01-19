using Ciandt.Retail.MCP.Models.Result;

namespace Ciandt.Retail.MCP.Models.ModelExtensions;
public static class ProductDetailResultExtension
{
    public static ProductDetailResult ToProductDetailResult(this ProductSummary prod)
    {
        return new ProductDetailResult()
        {
            AvailableColors = prod.AvailableColors,
            AvailableSizes = prod.AvailableSizes,
            AverageRating = prod.AverageRating,
            Brand = prod.Brand,
            Category = prod.Category,
            DiscountedPrice = prod.DiscountedPrice,
            HasPromotion = prod.HasPromotion,
            ImageUrl = prod.ImageUrl,
            InStock = prod.InStock,
            IsBestSeller = prod.IsBestSeller,
            IsNew = prod.IsNew,
            Name = prod.Name,
            Price = prod.Price,
            ProductId = prod.ProductId,
            PromoLabel = prod.PromoLabel,
            ReviewCount = prod.ReviewCount
        };
    }
}
