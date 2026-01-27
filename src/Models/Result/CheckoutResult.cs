namespace Ciandt.Retail.MCP.Models.Result;

public class CheckoutResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? OrderId { get; set; }
    public decimal Subtotal { get; set; }
    public decimal ShippingCost { get; set; }
    public decimal TotalAmount { get; set; }
    public List<CartItem> Items { get; set; } = new();
    public DateTime? CreatedAt { get; set; }

    public static CheckoutResult Succeeded(
        string orderId,
        decimal subtotal,
        decimal shippingCost,
        decimal totalAmount,
        List<CartItem> items)
    {
        return new CheckoutResult
        {
            Success = true,
            Message = "Pedido criado com sucesso!",
            OrderId = orderId,
            Subtotal = subtotal,
            ShippingCost = shippingCost,
            TotalAmount = totalAmount,
            Items = items,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static CheckoutResult Failed(string message)
    {
        return new CheckoutResult
        {
            Success = false,
            Message = message,
            OrderId = null,
            Subtotal = 0,
            ShippingCost = 0,
            TotalAmount = 0,
            Items = new List<CartItem>(),
            CreatedAt = null
        };
    }

    // Mantém compatibilidade com o método antigo
    [Obsolete("Use Succeeded(orderId, subtotal, shippingCost, totalAmount, items) instead")]
    public static CheckoutResult Succeeded(Cart cart, string checkoutId)
    {
        var subtotal = cart.Items.Sum(i => i.Price * i.Quantity);
        return new CheckoutResult
        {
            Success = true,
            Message = "Checkout preparado com sucesso",
            OrderId = checkoutId,
            Subtotal = subtotal,
            ShippingCost = 0,
            TotalAmount = subtotal,
            Items = cart.Items.ToList(),
            CreatedAt = DateTime.UtcNow
        };
    }
}