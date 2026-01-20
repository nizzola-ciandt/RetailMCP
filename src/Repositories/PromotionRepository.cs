using Ciandt.Retail.MCP.Data;
using Ciandt.Retail.MCP.Interfaces.Repositories;
using Ciandt.Retail.MCP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ciandt.Retail.MCP.Repositories;

public class PromotionRepository : IPromotionRepository
{
    private readonly RetailDbContext _context;
    private readonly ILogger<PromotionRepository> _logger;

    public PromotionRepository(RetailDbContext context, ILogger<PromotionRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ICollection<PromotionEntity>> GetActivePromotionsAsync(string? categoryId = null)
    {
        try
        {
            var now = DateTime.UtcNow;
            var query = _context.Promotions
                .Where(p => p.IsActive && p.StartDate <= now && p.EndDate >= now);

            if (!string.IsNullOrEmpty(categoryId))
            {
                query = query.Where(p => p.CategoryId == categoryId || p.CategoryId == null);
            }

            return await query.OrderByDescending(p => p.DiscountPercentage).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting active promotions");
            return new List<PromotionEntity>();
        }
    }

    public async Task<PromotionEntity?> GetPromotionByIdAsync(string promotionId)
    {
        try
        {
            return await _context.Promotions
                .FirstOrDefaultAsync(p => p.PromotionId == promotionId && p.IsActive);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting promotion: {promotionId}");
            return null;
        }
    }

    public async Task<ICollection<PromotionEntity>> GetPromotionsByProductIdAsync(string productId)
    {
        try
        {
            var now = DateTime.UtcNow;
            return await _context.Promotions
                .Where(p => p.IsActive
                    && p.StartDate <= now
                    && p.EndDate >= now
                    && (p.ProductId == productId || p.ProductId == null))
                .OrderByDescending(p => p.DiscountPercentage)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting promotions for product: {productId}");
            return new List<PromotionEntity>();
        }
    }

    public async Task<CouponEntity?> GetCouponByCodeAsync(string couponCode)
    {
        try
        {
            var now = DateTime.UtcNow;
            return await _context.Coupons
                .FirstOrDefaultAsync(c => c.CouponCode.ToUpper() == couponCode.ToUpper()
                    && c.IsActive
                    && c.StartDate <= now
                    && c.EndDate >= now
                    && (c.MaxUses == null || c.CurrentUses < c.MaxUses));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting coupon: {couponCode}");
            return null;
        }
    }

    public async Task<bool> ValidateCouponAsync(string couponCode, decimal cartTotal)
    {
        try
        {
            var coupon = await GetCouponByCodeAsync(couponCode);

            if (coupon == null)
            {
                _logger.LogWarning($"Coupon not found or invalid: {couponCode}");
                return false;
            }

            if (coupon.MinimumPurchaseAmount.HasValue && cartTotal < coupon.MinimumPurchaseAmount.Value)
            {
                _logger.LogWarning($"Cart total below minimum for coupon: {couponCode}");
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error validating coupon: {couponCode}");
            return false;
        }
    }

    public async Task<bool> UseCouponAsync(string couponCode)
    {
        try
        {
            var coupon = await GetCouponByCodeAsync(couponCode);

            if (coupon == null)
            {
                return false;
            }

            coupon.CurrentUses++;
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Coupon used: {couponCode}, Total uses: {coupon.CurrentUses}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error using coupon: {couponCode}");
            return false;
        }
    }

    public async Task<decimal> CalculateDiscountAsync(string? couponCode, decimal cartTotal)
    {
        try
        {
            if (string.IsNullOrEmpty(couponCode))
            {
                return 0;
            }

            var coupon = await GetCouponByCodeAsync(couponCode);

            if (coupon == null)
            {
                return 0;
            }

            if (coupon.MinimumPurchaseAmount.HasValue && cartTotal < coupon.MinimumPurchaseAmount.Value)
            {
                return 0;
            }

            decimal discount = 0;

            if (coupon.DiscountAmount.HasValue)
            {
                discount = coupon.DiscountAmount.Value;
            }
            else if (coupon.DiscountPercentage > 0)
            {
                discount = cartTotal * (coupon.DiscountPercentage / 100);
            }

            // Garante que o desconto não seja maior que o total do carrinho
            return Math.Min(discount, cartTotal);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating discount");
            return 0;
        }
    }

    public async Task<string> CreatePromotionAsync(PromotionEntity promotion)
    {
        try
        {
            promotion.PromotionId = Guid.NewGuid().ToString("N");
            promotion.CreatedAt = DateTime.UtcNow;

            _context.Promotions.Add(promotion);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Promotion created: {promotion.PromotionId}");
            return promotion.PromotionId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating promotion");
            throw;
        }
    }

    public async Task<string> CreateCouponAsync(CouponEntity coupon)
    {
        try
        {
            coupon.CouponCode = coupon.CouponCode.ToUpper();
            coupon.CreatedAt = DateTime.UtcNow;
            coupon.CurrentUses = 0;

            _context.Coupons.Add(coupon);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Coupon created: {coupon.CouponCode}");
            return coupon.CouponCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating coupon");
            throw;
        }
    }

    public async Task<bool> DeactivatePromotionAsync(string promotionId)
    {
        try
        {
            var promotion = await _context.Promotions.FindAsync(promotionId);

            if (promotion == null)
            {
                _logger.LogWarning($"Promotion not found: {promotionId}");
                return false;
            }

            promotion.IsActive = false;
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Promotion deactivated: {promotionId}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deactivating promotion: {promotionId}");
            return false;
        }
    }

    public async Task<bool> DeactivateCouponAsync(string couponCode)
    {
        try
        {
            var coupon = await _context.Coupons.FindAsync(couponCode.ToUpper());

            if (coupon == null)
            {
                _logger.LogWarning($"Coupon not found: {couponCode}");
                return false;
            }

            coupon.IsActive = false;
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Coupon deactivated: {couponCode}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deactivating coupon: {couponCode}");
            return false;
        }
    }
}