using Ciandt.Retail.MCP.Interfaces.Repositories;
using Ciandt.Retail.MCP.Interfaces.Services;
using Ciandt.Retail.MCP.Models;
using Ciandt.Retail.MCP.Models.Result;

namespace Ciandt.Retail.MCP.Services;

public class InvoiceService : IInvoiceService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly ILogger _logger;
    public InvoiceService(IPaymentRepository paymentRepository, IInvoiceRepository invoiceRepository, ILogger<InvoiceService> logger)
    {
        _paymentRepository = paymentRepository;
        _invoiceRepository = invoiceRepository;
        _logger = logger;
    }
    public Task<ReturnRequestResult> CreateReturnRequestAsync(string orderId, string productId, string reason)
    {
        throw new NotImplementedException();
    }

    public Task<OrderTrackingResult> GetOrderTrackingInfoAsync(string orderId)
    {
        throw new NotImplementedException();
    }

    public async Task<PaymentMethodsResult> GetPaymentOptionsAsync(string userId)
    {
        var paymentMethods =  await _paymentRepository.GetPaymentMethodsAsync(userId);
        return new PaymentMethodsResult()
        {
            AvailableMethods = paymentMethods
        };
    }

    public async Task<ShippingOptionsResult> GetShippingOptionsAsync(string userId, string zipCode)
    {
        var shipOptions = await _invoiceRepository.GetShippingOptions(userId, zipCode);
        return new ShippingOptionsResult()
        {
            Options = shipOptions,
            ZipCode = zipCode,
            EstimatedDeliveryTimeFrame = shipOptions.First().EstimatedDeliveryDays.ToString() + " days"
        };
    }
    public Task<RefundStatusResult> GetRefundStatusAsync(string refundId)
    {
        throw new NotImplementedException();
    }

    public Task<OrderIssueResult> RegisterOrderIssueAsync(string orderId, string issueDescription)
    {
        throw new NotImplementedException();
    }

    public async Task<decimal> CalculatePaymentValue(string userId, decimal totalValue, string paymentMethodId, int installments)
    {
        var paymentMethods = await _paymentRepository.GetPaymentMethodsAsync(userId);
        var selectedPayment = paymentMethods.Where(a => a.Id == paymentMethodId).FirstOrDefault();
        if (selectedPayment == null)
            return totalValue;

        return CalculateInstallments(totalValue, installments, selectedPayment.HandlingFee.Value);
    }

    public decimal CalculateInstallments(decimal totalValue, int installments, decimal handlingFee)
    {
        return (totalValue / installments);
    }
}
