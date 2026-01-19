namespace Ciandt.Retail.MCP.Models.Result;

public class CartUpdateResult
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public Cart Cart { get; set; }
    public decimal Total { get; set; }

    public static CartUpdateResult Succeeded(Cart cart)
    {
        return new CartUpdateResult
        {
            Success = true,
            Message = "Carrinho atualizado com sucesso.",
            Cart = cart,
            Total = cart.Items.Sum(i => i.Price * i.Quantity)
        };
    }

    public static CartUpdateResult Failed(string message)
    {
        return new CartUpdateResult
        {
            Success = false,
            Message = message
        };
    }
}
