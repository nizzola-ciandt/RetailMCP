using Ciandt.Retail.MCP.Interfaces.Repositories;
using Ciandt.Retail.MCP.Interfaces.Services;
using Ciandt.Retail.MCP.Models.Result;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace Ciandt.Retail.MCP.Tools;

[McpServerToolType]
public class PromotionTool
{
    private readonly ILogger<PromotionTool> _logger;
    private readonly IPromotionRepository _promotionRepository;
    private readonly ICartRepository _cartRepository;

    public PromotionTool(
        IPromotionRepository promotionRepository,
        ICartRepository cartRepository,
        ILogger<PromotionTool> logger)
    {
        _logger = logger;
        _promotionRepository = promotionRepository;
        _cartRepository = cartRepository;
    }

    [McpServerTool(Name = "promotion_inquiry", Title = "Customer seeks information about active promotions, discounts, or special offers")]
    [Description("Provides information about current promotions and applicable discounts, use parameter categoryId or 'all' to get all promotions.")]
    public async Task<PromotionResult> GetActivePromotionsAsync(string? categoryId = null)
    {
        try
        {
            _logger.LogInformation($"Retrieving active promotions for category: {categoryId ?? "all"}");

            var promotions = await _promotionRepository.GetActivePromotionsAsync(
                categoryId == "all" || string.IsNullOrEmpty(categoryId) ? null : categoryId);

            var promotionList = promotions.Select(p => new PromotionInfo
            {
                PromotionId = p.PromotionId,
                Title = p.Title,
                Description = p.Description ?? string.Empty,
                DiscountPercentage = p.DiscountPercentage,
                DiscountAmount = p.DiscountAmount,
                CategoryId = p.CategoryId,
                ProductId = p.ProductId,
                StartDate = p.StartDate,
                EndDate = p.EndDate
            }).ToList();

            return new PromotionResult
            {
                Success = true,
                Message = $"Found {promotionList.Count} active promotion(s)",
                Promotions = promotionList
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving promotions");
            return new PromotionResult
            {
                Success = false,
                Message = $"Error: {ex.Message}",
                Promotions = new List<PromotionInfo>()
            };
        }
    }

    [McpServerTool(Name = "coupon_application", Title = "Customer wants to apply a discount coupon to their cart or verify its validity")]
    [Description("Validates a coupon code and applies it to the customer's cart if valid.")]
    public async Task<CouponValidationResult> ValidateAndApplyCouponAsync(string userId, string couponCode)
    {
        try
        {
            _logger.LogInformation($"Validating coupon '{couponCode}' for user {userId}");

            // Busca o carrinho do usuário
            var cart = await _cartRepository.GetCartAsync(userId);
            if (cart == null || !cart.Items.Any())
            {
                return new CouponValidationResult
                {
                    IsValid = false,
                    Message = "Cart is empty. Add items before applying a coupon.",
                    DiscountAmount = 0
                };
            }

            // Calcula o total do carrinho
            var cartTotal = cart.Items.Sum(i => i.Price * i.Quantity);

            // Valida o cupom
            var isValid = await _promotionRepository.ValidateCouponAsync(couponCode, cartTotal);

            if (!isValid)
            {
                var coupon = await _promotionRepository.GetCouponByCodeAsync(couponCode);

                if (coupon == null)
                {
                    return new CouponValidationResult
                    {
                        IsValid = false,
                        Message = "Coupon code not found or expired.",
                        DiscountAmount = 0
                    };
                }

                if (coupon.MinimumPurchaseAmount.HasValue && cartTotal < coupon.MinimumPurchaseAmount.Value)
                {
                    return new CouponValidationResult
                    {
                        IsValid = false,
                        Message = $"Minimum purchase amount is {coupon.MinimumPurchaseAmount.Value:C}. Your cart total is {cartTotal:C}.",
                        DiscountAmount = 0,
                        MinimumPurchaseAmount = coupon.MinimumPurchaseAmount.Value
                    };
                }

                return new CouponValidationResult
                {
                    IsValid = false,
                    Message = "Coupon is not valid for your cart.",
                    DiscountAmount = 0
                };
            }

            // Calcula o desconto
            var discountAmount = await _promotionRepository.CalculateDiscountAsync(couponCode, cartTotal);

            // Marca o cupom como usado
            await _promotionRepository.UseCouponAsync(couponCode);

            return new CouponValidationResult
            {
                IsValid = true,
                Message = "Coupon applied successfully!",
                CouponCode = couponCode,
                DiscountAmount = discountAmount,
                FinalAmount = cartTotal - discountAmount
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error validating coupon: {couponCode}");
            return new CouponValidationResult
            {
                IsValid = false,
                Message = $"Error: {ex.Message}",
                DiscountAmount = 0
            };
        }
    }
}