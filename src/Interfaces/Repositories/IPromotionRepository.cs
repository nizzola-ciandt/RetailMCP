using Ciandt.Retail.MCP.Models.Entities;

namespace Ciandt.Retail.MCP.Interfaces.Repositories;

public interface IPromotionRepository
{
    Task<ICollection<PromotionEntity>> GetActivePromotionsAsync(string? categoryId = null);
    Task<PromotionEntity?> GetPromotionByIdAsync(string promotionId);
    Task<ICollection<PromotionEntity>> GetPromotionsByProductIdAsync(int ProductId);
    Task<CouponEntity?> GetCouponByCodeAsync(string couponCode);
    Task<bool> ValidateCouponAsync(string couponCode, decimal cartTotal);
    Task<bool> UseCouponAsync(string couponCode);
    Task<decimal> CalculateDiscountAsync(string? couponCode, decimal cartTotal);
    Task<string> CreatePromotionAsync(PromotionEntity promotion);
    Task<string> CreateCouponAsync(CouponEntity coupon);
    Task<bool> DeactivatePromotionAsync(string promotionId);
    Task<bool> DeactivateCouponAsync(string couponCode);
}