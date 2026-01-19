using Ciandt.Retail.MCP.Models.Result;

namespace Ciandt.Retail.MCP.Interfaces.Services;

public interface IPaymentService
{
    Task<PaymentMethodsResult> GetPaymentMethods(string userId);
}