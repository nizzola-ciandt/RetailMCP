using Ciandt.Retail.MCP.Interfaces.Repositories;
using Ciandt.Retail.MCP.Models;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace Ciandt.Retail.MCP.Tools;

[McpServerToolType]
public class PaymentTool
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<PaymentTool> _logger;

    public PaymentTool(
        IPaymentRepository paymentRepository,
        IOrderRepository orderRepository,
        ILogger<PaymentTool> logger)
    {
        _paymentRepository = paymentRepository;
        _orderRepository = orderRepository;
        _logger = logger;
    }

    [McpServerTool(Name = "payment_request", Title = "Customer wants to pay partial or complete value of your order and insert into payment queue")]
    [Description("Customer wants to pay partial or complete value of your order and insert into payment queue")]
    public async Task<PaymentProcessingResult> RequestPaymentAsync(int orderId, string paymentMethodId, decimal value, int installments = 1)
    {
        try
        {
            _logger.LogInformation($"Processing payment for order {orderId}: {value} via {paymentMethodId}");

            // Verifica se o pedido existe
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                return new PaymentProcessingResult
                {
                    Success = false,
                    Message = "Order not found"
                };
            }

            // Cria o pagamento
            var paymentId = await _paymentRepository.CreatePaymentAsync(orderId, paymentMethodId, value, installments);

            var payment = await _paymentRepository.GetPaymentByIdAsync(paymentId);

            return new PaymentProcessingResult
            {
                Success = true,
                PaymentId = paymentId,
                PaymentUrl = payment?.PaymentUrl ?? "https://payment.example.com",
                PaymentStatus = "Pending",
                Message = "Payment created successfully. Please complete the payment using the provided URL."
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error processing payment for order: {orderId}");
            return new PaymentProcessingResult
            {
                Success = false,
                Message = $"Error: {ex.Message}"
            };
        }
    }
}