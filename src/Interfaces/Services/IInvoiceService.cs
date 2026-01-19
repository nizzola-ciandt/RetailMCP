using Ciandt.Retail.MCP.Models.Result;

namespace Ciandt.Retail.MCP.Interfaces.Services;
public interface IInvoiceService
{
    /*
    Task<List<OrderSummary>> GetCustomerOrdersAsync(string customerId, int limit = 10, OrderFilters filters = null);
    Task<OrderStatus> GetOrderStatusAsync(string orderId, string customerId);
    Task<CheckoutSession> InitiateCheckoutAsync(string cartId);
    Task<PaymentProcessingResult> ProcessPaymentAsync(string orderId, PaymentData paymentData);
    */
    Task<ShippingOptionsResult> GetShippingOptionsAsync(string userId, string zipCode);

    Task<OrderTrackingResult> GetOrderTrackingInfoAsync(string orderId);

    Task<OrderIssueResult> RegisterOrderIssueAsync(string orderId, string issueDescription);

    Task<ReturnRequestResult> CreateReturnRequestAsync(string orderId, string productId, string reason);

    Task<RefundStatusResult> GetRefundStatusAsync(string refundId);

    Task<PaymentMethodsResult> GetPaymentOptionsAsync(string userId);
    Task<decimal> CalculatePaymentValue(string userId, decimal totalValue, string paymentMethodId, int Installments);
}