namespace Ciandt.Retail.MCP.Models.Result;

public class PaymentMethodsResult
{
    public ICollection<PaymentMethod> AvailableMethods { get; set; } = new List<PaymentMethod>();
    public ICollection<SavedPaymentMethod> SavedMethods { get; set; } = new List<SavedPaymentMethod>();
    public ICollection<PaymentPromotion> PaymentPromotions { get; set; } = new List<PaymentPromotion>();
    public string PreferredPaymentMethodId { get; set; }
}
