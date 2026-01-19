namespace Ciandt.Retail.MCP.Models.Result;

public class PaymentSelectedResult
{
    public int Installments { get; set; }
    public decimal InstallmentValue { get; set; }
    public string PaymentMethodId { get; set; }
    public decimal TotalValue { get { return Installments * InstallmentValue; } }
}
