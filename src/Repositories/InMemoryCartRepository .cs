using Ciandt.Retail.MCP.Interfaces.Repositories;
using Ciandt.Retail.MCP.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Ciandt.Retail.MCP.Repositories;

public class InMemoryCartRepository : ICartRepository
{
    private readonly IMemoryCache _cache;
    private readonly ILogger<InMemoryCartRepository> _logger;
    private readonly TimeSpan _cacheExpirationTime = TimeSpan.FromHours(24);

    public InMemoryCartRepository(IMemoryCache cache, ILogger<InMemoryCartRepository> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public Task<Cart> GetCartAsync(string userId)
    {
        _logger.LogInformation($"Obtendo carrinho para o usuário: {userId}");

        if (_cache.TryGetValue<Cart>($"cart_{userId}", out var cart))
        {
            return Task.FromResult(cart);
        }

        // Se não existir, cria um novo
        cart = new Cart
        {
            UserId = userId,
            Items = new List<CartItem>(),
            LastUpdated = DateTime.UtcNow
        };

        _cache.Set($"cart_{userId}", cart, _cacheExpirationTime);
        return Task.FromResult(cart);
    }

    public Task<Cart> UpdateCartAsync(Cart cart)
    {
        _logger.LogInformation($"Atualizando carrinho para o usuário: {cart.UserId}");

        cart.LastUpdated = DateTime.UtcNow;
        _cache.Set($"cart_{cart.UserId}", cart, _cacheExpirationTime);

        return Task.FromResult(cart);
    }

    public Task<bool> DeleteCartAsync(string userId)
    {
        _logger.LogInformation($"Removendo carrinho para o usuário: {userId}");

        _cache.Remove($"cart_{userId}");
        return Task.FromResult(true);
    }
}

