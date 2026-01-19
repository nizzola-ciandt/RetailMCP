namespace Ciandt.Retail.MCP.Models.Request;

public class CustomerCreateRequest
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Zip { get; set; }
    public string DocumentNumber { get; set; }
}
