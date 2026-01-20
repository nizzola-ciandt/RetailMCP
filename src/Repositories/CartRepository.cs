using Ciandt.Retail.MCP.Data;
using Ciandt.Retail.MCP.Interfaces.Repositories;
using Ciandt.Retail.MCP.Models;
using Ciandt.Retail.MCP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ciandt.Retail.MCP.Repositories;

public class CartRepository : ICartRepository
{
    private readonly RetailDbContext _context;
    private readonly ILogger<CartRepository> _logger;

    public CartRepository(RetailDbContext context, ILogger<CartRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Cart> GetCartAsync(string userId)
    {
        try
        {
            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var customerId))
            {
                _logger.LogWarning("Invalid user ID");
                return CreateEmptyCart(userId);
            }

            var cartEntity = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);

            if (cartEntity == null)
            {
                // Criar um novo carrinho
                cartEntity = new CartEntity
                {
                    CustomerId = customerId,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdated = DateTime.UtcNow
                };

                _context.Carts.Add(cartEntity);
                await _context.SaveChangesAsync();
            }

            return new Cart
            {
                UserId = userId,
                Items = cartEntity.Items.Select(i => new CartItem
                {
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    Price = i.Price,
                    Quantity = i.Quantity
                }).ToList(),
                LastUpdated = cartEntity.LastUpdated
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting cart for user: {userId}");
            return CreateEmptyCart(userId);
        }
    }

    public async Task<Cart> UpdateCartAsync(Cart cart)
    {
        try
        {
            if (string.IsNullOrEmpty(cart.UserId) || !int.TryParse(cart.UserId, out var customerId))
            {
                _logger.LogWarning("Invalid user ID");
                return cart;
            }

            var cartEntity = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);

            if (cartEntity == null)
            {
                _logger.LogWarning($"Cart not found for user: {cart.UserId}");
                return cart;
            }

            // Remove todos os itens existentes
            _context.CartItems.RemoveRange(cartEntity.Items);

            // Adiciona os novos itens
            foreach (var item in cart.Items)
            {
                cartEntity.Items.Add(new CartItemEntity
                {
                    CartId = cartEntity.Id,
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    AddedAt = DateTime.UtcNow
                });
            }

            cartEntity.LastUpdated = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            cart.LastUpdated = cartEntity.LastUpdated;
            return cart;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating cart for user: {cart.UserId}");
            return cart;
        }
    }

    public async Task<bool> DeleteCartAsync(string userId)
    {
        try
        {
            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var customerId))
            {
                _logger.LogWarning("Invalid user ID");
                return false;
            }

            var cartEntity = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);

            if (cartEntity == null)
            {
                _logger.LogWarning($"Cart not found for user: {userId}");
                return false;
            }

            _context.Carts.Remove(cartEntity);
            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting cart for user: {userId}");
            return false;
        }
    }

    private Cart CreateEmptyCart(string userId)
    {
        return new Cart
        {
            UserId = userId,
            Items = new List<CartItem>(),
            LastUpdated = DateTime.UtcNow
        };
    }
}