using Ciandt.Retail.MCP.Data;
using Ciandt.Retail.MCP.Interfaces.Repositories;
using Ciandt.Retail.MCP.Models;
using Ciandt.Retail.MCP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ciandt.Retail.MCP.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly RetailDbContext _context;
    private readonly ILogger<PaymentRepository> _logger;

    public PaymentRepository(RetailDbContext context, ILogger<PaymentRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ICollection<PaymentMethod>> GetPaymentMethodsAsync(string userId)
    {
        // Retorna os métodos de pagamento disponíveis (hardcoded por enquanto)
        // Pode ser posteriormente carregado de uma tabela de configuração
        return await Task.FromResult(GetAvailablePaymentMethods());
    }

    public async Task<string> CreatePaymentAsync(int orderId, string paymentMethodId, decimal amount, int installments = 1)
    {
        try
        {
            var paymentId = Guid.NewGuid().ToString("N");

            var payment = new PaymentEntity
            {
                PaymentId = paymentId,
                OrderId = orderId,
                PaymentMethodId = paymentMethodId,
                Amount = amount,
                Installments = installments,
                InstallmentValue = amount / installments,
                Status = "Pending",
                PaymentUrl = GeneratePaymentUrl(paymentMethodId, paymentId),
                CreatedAt = DateTime.UtcNow
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Payment created: {paymentId} for order: {orderId}");
            return paymentId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating payment");
            throw;
        }
    }

    public async Task<PaymentEntity?> GetPaymentByIdAsync(string paymentId)
    {
        try
        {
            return await _context.Payments
                .Include(p => p.Order)
                .FirstOrDefaultAsync(p => p.PaymentId == paymentId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting payment: {paymentId}");
            return null;
        }
    }

    public async Task<ICollection<PaymentEntity>> GetPaymentsByOrderIdAsync(int orderId)
    {
        try
        {
            return await _context.Payments
                .Where(p => p.OrderId == orderId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting payments for order: {orderId}");
            return new List<PaymentEntity>();
        }
    }

    public async Task<bool> UpdatePaymentStatusAsync(string paymentId, string newStatus, string? transactionId = null)
    {
        try
        {
            var payment = await _context.Payments.FindAsync(paymentId);
            if (payment == null)
            {
                _logger.LogWarning($"Payment not found: {paymentId}");
                return false;
            }

            payment.Status = newStatus;
            if (!string.IsNullOrEmpty(transactionId))
            {
                payment.TransactionId = transactionId;
            }

            if (newStatus == "Approved" || newStatus == "Declined" || newStatus == "Refunded")
            {
                payment.ProcessedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation($"Payment status updated: {paymentId} -> {newStatus}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating payment status: {paymentId}");
            return false;
        }
    }

    public async Task<bool> ProcessRefundAsync(string paymentId, decimal refundAmount)
    {
        try
        {
            var payment = await _context.Payments.FindAsync(paymentId);
            if (payment == null)
            {
                _logger.LogWarning($"Payment not found: {paymentId}");
                return false;
            }

            if (payment.Status != "Approved")
            {
                _logger.LogWarning($"Payment not in approved status: {paymentId}");
                return false;
            }

            if (refundAmount > payment.Amount)
            {
                _logger.LogWarning($"Refund amount exceeds payment amount: {paymentId}");
                return false;
            }

            payment.Status = "Refunded";
            payment.ProcessedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            _logger.LogInformation($"Refund processed for payment: {paymentId}, Amount: {refundAmount}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error processing refund: {paymentId}");
            return false;
        }
    }

    private string GeneratePaymentUrl(string paymentMethodId, string paymentId)
    {
        // Gera URL de pagamento baseada no método
        return paymentMethodId.ToLower() switch
        {
            "pix" => $"https://payment.example.com/pix/{paymentId}",
            "boleto" => $"https://payment.example.com/boleto/{paymentId}",
            "paypal" => $"https://paypal.com/checkout/{paymentId}",
            _ => $"https://payment.example.com/checkout/{paymentId}"
        };
    }

    private List<PaymentMethod> GetAvailablePaymentMethods()
    {
        return new List<PaymentMethod>
        {
            // Cartões de crédito
            new PaymentMethod
            {
                Id = "visa_credit",
                Name = "Visa",
                Type = "Credit",
                Description = "Cartão de crédito Visa",
                SupportsInstallments = true,
                MaxInstallments = 12,
                HandlingFee = 0.029m,
                IconUrl = "/assets/payment-icons/visa.svg"
            },
            new PaymentMethod
            {
                Id = "mastercard_credit",
                Name = "Mastercard",
                Type = "Credit",
                Description = "Cartão de crédito Mastercard",
                SupportsInstallments = true,
                MaxInstallments = 12,
                HandlingFee = 0.031m,
                IconUrl = "/assets/payment-icons/mastercard.svg"
            },
            new PaymentMethod
            {
                Id = "amex_credit",
                Name = "American Express",
                Type = "Credit",
                Description = "Cartão de crédito American Express",
                SupportsInstallments = true,
                MaxInstallments = 10,
                HandlingFee = 0.034m,
                IconUrl = "/assets/payment-icons/amex.svg"
            },
            
            // Cartões de débito
            new PaymentMethod
            {
                Id = "visa_debit",
                Name = "Visa Débito",
                Type = "Debit",
                Description = "Cartão de débito Visa",
                SupportsInstallments = false,
                HandlingFee = 0.019m,
                IconUrl = "/assets/payment-icons/visa-debit.svg"
            },
            new PaymentMethod
            {
                Id = "mastercard_debit",
                Name = "Mastercard Débito",
                Type = "Debit",
                Description = "Cartão de débito Mastercard",
                SupportsInstallments = false,
                HandlingFee = 0.018m,
                IconUrl = "/assets/payment-icons/mastercard-debit.svg"
            },
            
            // Carteiras digitais
            new PaymentMethod
            {
                Id = "paypal",
                Name = "PayPal",
                Type = "PayPal",
                Description = "Pagamento via PayPal",
                SupportsInstallments = false,
                HandlingFee = 0.039m,
                IconUrl = "/assets/payment-icons/paypal.svg"
            },
            new PaymentMethod
            {
                Id = "pix",
                Name = "PIX",
                Type = "Instant",
                Description = "Pagamento instantâneo via PIX",
                SupportsInstallments = false,
                HandlingFee = 0.01m,
                IconUrl = "/assets/payment-icons/pix.svg"
            },
            
            // Métodos de boleto e transferência
            new PaymentMethod
            {
                Id = "boleto",
                Name = "Boleto Bancário",
                Type = "Boleto",
                Description = "Pagamento via boleto bancário",
                SupportsInstallments = false,
                HandlingFee = 0.01m,
                IconUrl = "/assets/payment-icons/boleto.svg"
            },
            new PaymentMethod
            {
                Id = "bank_transfer",
                Name = "Transferência Bancária",
                Type = "BankTransfer",
                Description = "Pagamento via transferência bancária",
                SupportsInstallments = false,
                HandlingFee = 0.005m,
                IconUrl = "/assets/payment-icons/bank-transfer.svg"
            },
            
            // Métodos internacionais
            new PaymentMethod
            {
                Id = "apple_pay",
                Name = "Apple Pay",
                Type = "DigitalWallet",
                Description = "Pagamento via Apple Pay",
                SupportsInstallments = false,
                HandlingFee = 0.025m,
                IconUrl = "/assets/payment-icons/apple-pay.svg"
            },
            new PaymentMethod
            {
                Id = "google_pay",
                Name = "Google Pay",
                Type = "DigitalWallet",
                Description = "Pagamento via Google Pay",
                SupportsInstallments = false,
                HandlingFee = 0.025m,
                IconUrl = "/assets/payment-icons/google-pay.svg"
            }
        };
    }
}