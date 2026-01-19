
using Ciandt.Retail.MCP.Models;

namespace Ciandt.Retail.MCP.Interfaces.Repositories;

public interface IInvoiceRepository
{
    Task<ICollection<ShippingOption>> GetShippingOptions(string userId, string zipCode);
}
