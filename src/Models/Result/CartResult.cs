namespace Ciandt.Retail.MCP.Models.Result;

public class CartResult : Cart
{
    public bool HasItems {  get { return Items.Count > 0; } }
    public decimal Total
    {
        get
        {
            return this.Items.Sum(a => a.Price * a.Quantity);
        }
    }
}
