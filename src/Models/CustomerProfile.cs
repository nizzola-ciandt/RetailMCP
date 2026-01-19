namespace Ciandt.Retail.MCP.Models;

public class CustomerProfile
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Zip { get; set; }
    
    public ICollection<Address> Address { get; set; } = new List<Address>();
}
