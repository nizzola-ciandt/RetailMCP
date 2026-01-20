using Ciandt.Retail.MCP.Interfaces.Repositories;
using Ciandt.Retail.MCP.Interfaces.Services;
using Ciandt.Retail.MCP.Models;
using Ciandt.Retail.MCP.Models.Entities;
using Ciandt.Retail.MCP.Models.Result;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace Ciandt.Retail.MCP.Tools;

[McpServerToolType]
public class OrderTool
{
    private readonly ILogger<OrderTool> _logger;
    private readonly IOrderRepository _orderRepository;
    private readonly ICartRepository _cartRepository;
    private readonly IPaymentRepository _paymentRepository;

    public OrderTool(
        IOrderRepository orderRepository,
        ICartRepository cartRepository,
        IPaymentRepository paymentRepository,
        ILogger<OrderTool> logger)
    {
        _logger = logger;
        _orderRepository = orderRepository;
        _cartRepository = cartRepository;
        _paymentRepository = paymentRepository;
    }

    [McpServerTool(Name = "order_status", Title = "Customer request status your order including delivery and payment information")]
    [Description("Customer request status your order including delivery and payment information")]
    public async Task<OrderSummary> RequestOrderStatusAsync(string orderId)
    {
        try
        {
            _logger.LogInformation($"Getting order status for: {orderId}");
            var orderSummary = await _orderRepository.GetOrderSummaryAsync(orderId);

            if (orderSummary == null || string.IsNullOrEmpty(orderSummary.OrderId))
            {
                _logger.LogWarning($"Order not found: {orderId}");
                return new OrderSummary();
            }

            return orderSummary;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting order status: {orderId}");
            return new OrderSummary();
        }
    }

    [McpServerTool(Name = "get_previous_orders", Title = "Customer request a list of previous orders")]
    [Description("Customer request a list of previous orders with pagination support")]
    public async Task<OrderListResult> RequestPreviousOrdersAsync(string userId, int page = 1, int pageSize = 10)
    {
        try
        {
            _logger.LogInformation($"Getting previous orders for user: {userId}");

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var customerId))
            {
                return new OrderListResult
                {
                    Success = false,
                    Message = "Invalid user ID",
                    Orders = new List<OrderSummary>()
                };
            }

            var orders = await _orderRepository.GetOrdersByCustomerIdAsync(customerId, page, pageSize);

            var orderSummaries = new List<OrderSummary>();
            foreach (var order in orders)
            {
                var summary = await _orderRepository.GetOrderSummaryAsync(order.OrderId);
                if (summary != null && !string.IsNullOrEmpty(summary.OrderId))
                {
                    orderSummaries.Add(summary);
                }
            }

            return new OrderListResult
            {
                Success = true,
                Message = $"Found {orderSummaries.Count} orders",
                Orders = orderSummaries,
                CurrentPage = page,
                PageSize = pageSize
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting previous orders: {userId}");
            return new OrderListResult
            {
                Success = false,
                Message = $"Error: {ex.Message}",
                Orders = new List<OrderSummary>()
            };
        }
    }

    [McpServerTool(Name = "order_tracking", Title = "Customer wants to monitor the status of a delivery or track an existing order")]
    [Description("Retrieves the current status and tracking information for a specific order.")]
    public async Task<OrderTrackingResult> TrackOrderAsync(string orderId)
    {
        try
        {
            _logger.LogInformation($"Tracking order: {orderId}");
            return await _orderRepository.GetOrderTrackingAsync(orderId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error tracking order: {orderId}");
            return new OrderTrackingResult
            {
                Success = false,
                Message = $"Error: {ex.Message}"
            };
        }
    }

    [McpServerTool(Name = "order_problem", Title = "Customer reports problems with an order")]
    [Description("Records a problem with an order and initiates the resolution process.")]
    public async Task<OrderIssueResult> ReportOrderIssueAsync(string orderId, string issueDescription)
    {
        try
        {
            _logger.LogInformation($"Reporting issue for order {orderId}: {issueDescription}");

            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                return new OrderIssueResult
                {
                    Success = false,
                    Message = "Order not found",
                    IssueId = string.Empty
                };
            }

            // Em uma implementação real, você criaria um registro de issue em uma tabela separada
            var issueId = Guid.NewGuid().ToString("N");

            _logger.LogInformation($"Issue reported successfully. Issue ID: {issueId}");

            return new OrderIssueResult
            {
                Success = true,
                Message = "Issue reported successfully. Our team will contact you shortly.",
                IssueId = issueId,
                ExpectedResolutionDate = DateTime.Now.AddDays(2) //ExpectedResolutionTime = "24-48 hours"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error reporting order issue: {orderId}");
            return new OrderIssueResult
            {
                Success = false,
                Message = $"Error: {ex.Message}",
                IssueId = string.Empty
            };
        }
    }

    [McpServerTool(Name = "return_request", Title = "Customer wants to initiate a product return process")]
    [Description("Initiates the return process for a purchased product.")]
    public async Task<ReturnRequestResult> InitiateReturnAsync(string orderId, string productId, string reason)
    {
        try
        {
            _logger.LogInformation($"Initiating return for product {productId} from order {orderId}");

            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                return new ReturnRequestResult
                {
                    Success = false,
                    Message = "Order not found"
                };
            }

            // Verifica se o pedido pode ser devolvido (não pode estar cancelado ou muito antigo)
            if (order.Status == OrderStatusEnum.Cancelled)
            {
                return new ReturnRequestResult
                {
                    Success = false,
                    Message = "Cannot return a cancelled order"
                };
            }

            var daysSinceDelivery = order.DeliveredAt.HasValue
                ? (DateTime.UtcNow - order.DeliveredAt.Value).Days
                : 0;

            if (daysSinceDelivery > 30)
            {
                return new ReturnRequestResult
                {
                    Success = false,
                    Message = "Return period has expired (30 days)"
                };
            }

            // Verifica se o produto está no pedido
            var orderItem = order.OrderItems.FirstOrDefault(i => i.ProductId == productId);
            if (orderItem == null)
            {
                return new ReturnRequestResult
                {
                    Success = false,
                    Message = "Product not found in this order"
                };
            }

            var returnId = Guid.NewGuid().ToString("N");

            _logger.LogInformation($"Return request created successfully. Return ID: {returnId}");

            return new ReturnRequestResult
            {
                Success = true,
                Message = "Return request created successfully",
                ReturnId = returnId,
                RefundAmount = orderItem.TotalPrice,
                ExpectedRefundDays = 7,
                ReturnInstructions = "Please package the item securely and drop it off at any authorized return location. You will receive a prepaid shipping label via email."
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error initiating return: {orderId}");
            return new ReturnRequestResult
            {
                Success = false,
                Message = $"Error: {ex.Message}"
            };
        }
    }

    [McpServerTool(Name = "refund_status", Title = "Customer seeks information about the status of a requested refund")]
    [Description("Checks the status of a refund request.")]
    public async Task<RefundStatusResult> CheckRefundStatusAsync(string refundId)
    {
        try
        {
            _logger.LogInformation($"Checking refund status for: {refundId}");

            // Em uma implementação real, você buscaria o status do refund em uma tabela
            // Por enquanto, retornamos um exemplo

            return new RefundStatusResult
            {
                Success = true,
                RefundId = refundId,
                Status = OrderStatusEnum.Processing,
                RefundAmount = 0, // Seria buscado do banco
                RefundMethod = "Original payment method",
                ExpectedCompletionDate = DateTime.UtcNow.AddDays(7),
                Message = "Your refund is being processed and will be completed within 7 business days."
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error checking refund status: {refundId}");
            return new RefundStatusResult
            {
                Success = false,
                Message = $"Error: {ex.Message}"
            };
        }
    }

    [McpServerTool(Name = "order_payment_status", Title = "Customer request status of all payments of your order")]
    [Description("Customer request status of all payments of your order")]
    public async Task<PaymentProcessingResult> RequestOrderPaymentStatusAsync(string orderId)
    {
        try
        {
            _logger.LogInformation($"Getting payment status for order: {orderId}");

            var payments = await _paymentRepository.GetPaymentsByOrderIdAsync(orderId);

            if (!payments.Any())
            {
                return new PaymentProcessingResult
                {
                    Success = false,
                    Message = "No payments found for this order"
                };
            }

            var paymentDetails = payments.Select(p => new
            {
                p.PaymentId,
                p.PaymentMethodId,
                p.Amount,
                p.Installments,
                p.Status,
                p.CreatedAt
            }).ToList();

            return new PaymentProcessingResult
            {
                Success = true,
                Message = $"Found {payments.Count} payment(s) for this order",
                PaymentStatus = payments.First().Status,
                PaymentDetails = System.Text.Json.JsonSerializer.Serialize(paymentDetails)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting payment status: {orderId}");
            return new PaymentProcessingResult
            {
                Success = false,
                Message = $"Error: {ex.Message}"
            };
        }
    }
}