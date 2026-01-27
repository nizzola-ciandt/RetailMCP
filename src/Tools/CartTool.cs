using Ciandt.Retail.MCP.Interfaces.Services;
using Ciandt.Retail.MCP.Models.Result;
using Ciandt.Retail.MCP.Services;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace Ciandt.Retail.MCP.Tools;

[McpServerToolType]
public class CartTool
{
    private readonly ILogger _logger;
    private readonly ICartService _cartService;
    private readonly IInvoiceService _invoiceService;

    public CartTool(ICartService cartService, IInvoiceService invoiceService, ILogger<CartTool> logger)
    {
        _logger = logger;
        _cartService = cartService;
        _invoiceService = invoiceService;
    }

    [McpServerTool(Name = "add_to_cart", Title = "Customer expresses interest in adding a product to their shopping cart")]
    [Description("Adds a specified product to the customer's shopping cart, required fields are: userId from user data, productId has the same name in product.")]
    public async Task<CartUpdateResult> AddProductToCartAsync(string userId, string productId, int quantity)
    {
        _logger.LogInformation($"Adding product {productId} (qty: {quantity}) to cart for user {userId}");
        return await _cartService.AddItemAsync(userId, productId, quantity);
    }

    [McpServerTool(Name = "cart_management", Title = "Customer wants to modify items in the cart, the field productId is the same provided in product list")]
    [Description("Updates quantities or removes items from the shopping cart.")]
    public async Task<CartUpdateResult> UpdateCartItemAsync(string userId, string productId, int newQuantity)
    {
        _logger.LogInformation($"Updating cart item {productId} to quantity {newQuantity} for user {userId}");
        return await _cartService.UpdateItemQuantityAsync(userId, productId, newQuantity);
    }

    [McpServerTool(Name = "checkout", Title = "Customer is ready to complete the purchase and proceed to payment")]
    [Description("Initiates the checkout process for the current cart.")]
    public async Task<CheckoutResult> InitiateCheckoutAsync(string userId)
    {
        _logger.LogInformation($"Initiating checkout for user {userId}");
        return await _cartService.PrepareCheckoutAsync(userId);
    }

    [McpServerTool(Name = "cart_list", Title = "Customer wants to check all items on your cart and total value, userId is the key to find user, use previously supplied by system or request user data to find correct user")]
    [Description("Customer wants to check all items on your cart and total value.")]
    public async Task<CartResult> ListCartAsync(string userId)
    {
        _logger.LogInformation($"Initiating cart list for user {userId}");
        return await _cartService.ListCartAsync(userId);
    }

    [McpServerTool(Name = "shipping_inquiry", Title = "Customer requests information about delivery options, timeframes, or shipping costs")]
    [Description("Calculates shipping options and costs for the customer's location and current cart.")]
    public async Task<ShippingOptionsResult> CalculateShippingOptionsAsync(string userId, string zipCode)
    {
        _logger.LogInformation($"Calculating shipping options to {zipCode} for user {userId}");
        return await _invoiceService.GetShippingOptionsAsync(userId, zipCode);
    }

    [McpServerTool(Name = "payment_method", Title = "Customer seeks information or wants to select a specific payment method")]
    [Description("Retrieves available payment methods or sets the preferred payment method for checkout.")]
    public async Task<PaymentMethodsResult> GetAvailablePaymentMethodsAsync(string userId)
    {
        _logger.LogInformation($"Retrieving payment methods for user {userId}");
        return await _invoiceService.GetPaymentOptionsAsync(userId);
    }

    [McpServerTool(Name = "calculate_payment", Title = "Customer select payment method and wants to know whats is the values")]
    [Description("Retrieves available payment methods or sets the preferred payment method for checkout.")]
    public async Task<PaymentSelectedResult> GetAvailablePaymentMethodsAsync(string userId, decimal totalValue, string paymentMethodId, int installments)
    {
        _logger.LogInformation($"Retrieving payment methods for user {userId}");
        var paymentValue= await _invoiceService.CalculatePaymentValue(userId, totalValue, paymentMethodId, installments);
        return new PaymentSelectedResult()
        {
            Installments = installments,
            InstallmentValue = paymentValue/installments,
            PaymentMethodId = paymentMethodId
        };
    }
}
