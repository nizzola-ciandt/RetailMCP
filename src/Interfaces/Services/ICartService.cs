using Ciandt.Retail.MCP.Models.Result;

namespace Ciandt.Retail.MCP.Interfaces.Services;

public interface ICartService
{
    Task<CartUpdateResult> AddItemAsync(string userId, string productId, int quantity);
    Task<CartUpdateResult> UpdateItemQuantityAsync(string userId, string productId, int newQuantity);
    Task<CheckoutResult> PrepareCheckoutAsync(string userId);
    Task<CartResult> ListCartAsync(string userId);
}
