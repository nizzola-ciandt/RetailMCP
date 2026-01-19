namespace Ciandt.Retail.MCP.Models;

public class OrderResult
{
    public int OrderId { get; set; }
    public DateTime PurchaseDate { get; set; }
    public int CustomerId { get; set; }

    public ICollection<OrderItems> Items { get; set; } = new List<OrderItems>();

    public ICollection<OrderPaymentData> Payments { get; set; } = new List<OrderPaymentData>();

    public OrderShippingInfo ShippingInfo {get; set; } = new OrderShippingInfo();
}

public class OrderItems
{
    public OrderProduct Product { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Discount { get; set; }
}

public class OrderProduct
{
    public int ProductId { get; set; }
    public string? Name { get; set; }
    public string? Color { get; set; }
    public string? Size { get; set; }
}

public class OrderPaymentData
{
    public string PaymentType { get; set; }
    public int PaymentMethodId { get; set; }
    public decimal Value { get; set; }
    public string Currency { get; set; }
}

public class OrderShippingInfo
{
    public int ShippingInfoId { get; set; }
    public DateTime? EstimatedDeliveryDate { get; set; }
    public decimal Value { get; set; }
}