using Ciandt.Retail.MCP.Models.Result;

namespace Ciandt.Retail.MCP.Interfaces.Services;

public interface ICartService
{
    Task<CartUpdateResult> AddItemAsync(string userId, int ProductId, int quantity);
    Task<CartUpdateResult> UpdateItemQuantityAsync(string userId, int ProductId, int newQuantity);
    Task<CheckoutResult> PrepareCheckoutAsync(string userId);
    Task<CartResult> ListCartAsync(string userId);
}
