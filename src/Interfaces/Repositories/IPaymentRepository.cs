using Ciandt.Retail.MCP.Models;
using Ciandt.Retail.MCP.Models.Entities;

namespace Ciandt.Retail.MCP.Interfaces.Repositories;

public interface IPaymentRepository
{
    Task<ICollection<PaymentMethod>> GetPaymentMethodsAsync(string userId);
    Task<PaymentEntity?> GetPaymentByIdAsync(string paymentId);
    Task<string> CreatePaymentAsync(string orderId, string paymentMethodId, decimal amount, int installments = 1);
    Task<ICollection<PaymentEntity>> GetPaymentsByOrderIdAsync(string orderId);
}
