using Ciandt.Retail.MCP.Models;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace Ciandt.Retail.MCP.Tools;

public class PaymentTool
{
    [McpServerTool(Name = "payment_request", Title = "Customer wants to pay partial or complete value of your order and insert into payment queue")]
    [Description("Customer wants to pay partial or complete value of your order and insert into payment queue")]
    public async Task<PaymentProcessingResult> RequestPaymentAsync(string orderId, int paymentMethodId, decimal value)
    {
        return new PaymentProcessingResult()
        {
            PaymentUrl = "http:\\www.ciandt.com",
            Message = "you can proceed payment and when you finish your payment, you receive tracking data"
        };
        //_logger.LogInformation($"Tracking order {orderId}");
        //return await _invoiceService.GetOrderTrackingInfoAsync(orderId);
    }
}
