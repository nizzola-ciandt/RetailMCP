using Ciandt.Retail.MCP.Models;
using Ciandt.Retail.MCP.Models.Result;

namespace Ciandt.Retail.MCP.Interfaces.Repositories;

public interface ICartRepository
{
    Task<Cart> GetCartAsync(string userId);
    Task<Cart> UpdateCartAsync(Cart cart);
    Task<bool> DeleteCartAsync(string userId);
}