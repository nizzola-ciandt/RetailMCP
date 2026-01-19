namespace Ciandt.Retail.MCP.Models.Result;

public class CheckoutResult
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public Cart Cart { get; set; }
    public decimal Total { get; set; }
    public string CheckoutId { get; set; }

    public static CheckoutResult Succeeded(Cart cart, string checkoutId)
    {
        return new CheckoutResult
        {
            Success = true,
            Message = "Checkout preparado com sucesso. Realize o pagamento!",
            Cart = cart,
            Total = cart.Items.Sum(i => i.Price * i.Quantity),
            CheckoutId = checkoutId
        };
    }

    public static CheckoutResult Failed(string message)
    {
        return new CheckoutResult
        {
            Success = false,
            Message = message
        };
    }
}
