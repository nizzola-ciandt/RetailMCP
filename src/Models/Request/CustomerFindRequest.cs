namespace Ciandt.Retail.MCP.Models.Request;

public class CustomerFindRequest
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; } 
    public string? DocumentCPF { get; set; }
}
