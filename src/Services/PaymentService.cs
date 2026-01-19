using Ciandt.Retail.MCP.Interfaces.Repositories;
using Ciandt.Retail.MCP.Interfaces.Services;
using Ciandt.Retail.MCP.Models.Result;

namespace Ciandt.Retail.MCP.Services;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    public PaymentService(IPaymentRepository paymentRepository)
    {
            _paymentRepository = paymentRepository;
    }
    public async Task<PaymentMethodsResult> GetPaymentMethods(string userId)
    {
        var payments = await _paymentRepository.GetPaymentMethodsAsync(userId);
        return new PaymentMethodsResult()
        {
            AvailableMethods = payments,
            PreferredPaymentMethodId = payments.First().Id
        };
    }
}
