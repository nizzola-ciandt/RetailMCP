namespace Ciandt.Retail.MCP.Models.Request;

public class CustomerAddressCreateRequest
{
    public string CustomerId { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public bool Default { get; set; } = false;
}
