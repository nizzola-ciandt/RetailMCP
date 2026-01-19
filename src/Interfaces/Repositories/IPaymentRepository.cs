using Ciandt.Retail.MCP.Models;

namespace Ciandt.Retail.MCP.Interfaces.Repositories;

public interface IPaymentRepository
{
    Task<ICollection<PaymentMethod>> GetPaymentMethodsAsync(string userId);
}
