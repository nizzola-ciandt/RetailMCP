namespace Ciandt.Retail.MCP.Models.Request;

public class CustomerUpdateRequest
{
    public string Id { get; set; }
    public string CustomerId { get; set; }
    public string Name { get; set; }
    public string Gender { get; set; }
    public string Email { get; set; }
    public string MobilePhone { get; set; }
    public string Zip { get; set; }
    public string Phone { get; set; }
}
