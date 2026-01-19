using Ciandt.Retail.MCP.Interfaces.Repositories;
using Ciandt.Retail.MCP.Interfaces.Services;
using Ciandt.Retail.MCP.Models;
using Ciandt.Retail.MCP.Models.Result;

namespace Ciandt.Retail.MCP.Services;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;
    private readonly ILogger _logger;

    public CartService(IProductRepository productsRepository, ICartRepository cartRepository, ILogger<CartService> logger)
    {
        _cartRepository = cartRepository;
        _productRepository = productsRepository;
        _logger = logger;
    }

    public async Task<CartUpdateResult> AddItemAsync(string userId, string productId, int quantity)
    {
        try
        {
            if (string.IsNullOrEmpty(userId))
                return CartUpdateResult.Failed("ID do usuário é obrigatório.");

            if (string.IsNullOrEmpty(productId))
                return CartUpdateResult.Failed("ID do produto é obrigatório.");

            if (quantity <= 0)
                return CartUpdateResult.Failed("Quantidade deve ser maior que zero.");

            var cart = await _cartRepository.GetCartAsync(userId);

            // Obtenha informações do produto
            var product = await _productRepository.GetProductDetailsAsync(productId);

            if (product == null)
                return CartUpdateResult.Failed("Produto não encontrado.");

            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.Items.Add(new CartItem
                {
                    ProductId = productId,
                    ProductName = product.Name,
                    Price = product.Price,
                    Quantity = quantity
                });
            }

            await _cartRepository.UpdateCartAsync(cart);
            _logger.LogInformation($"Item {productId} adicionado ao carrinho do usuário {userId}");

            return CartUpdateResult.Succeeded(cart);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao adicionar item ao carrinho: {ex.Message}");
            return CartUpdateResult.Failed("Ocorreu um erro ao adicionar o item ao carrinho.");
        }
    }

    public async Task<CartUpdateResult> UpdateItemQuantityAsync(string userId, string productId, int newQuantity)
    {
        try
        {
            if (string.IsNullOrEmpty(userId))
                return CartUpdateResult.Failed("ID do usuário é obrigatório.");

            if (string.IsNullOrEmpty(productId))
                return CartUpdateResult.Failed("ID do produto é obrigatório.");

            var cart = await _cartRepository.GetCartAsync(userId);
            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (item == null)
                return CartUpdateResult.Failed("Item não encontrado no carrinho.");

            if (newQuantity <= 0)
            {
                cart.Items.Remove(item);
                _logger.LogInformation($"Item {productId} removido do carrinho do usuário {userId}");
            }
            else
            {
                item.Quantity = newQuantity;
                _logger.LogInformation($"Quantidade do item {productId} atualizada para {newQuantity} no carrinho do usuário {userId}");
            }

            await _cartRepository.UpdateCartAsync(cart);

            return CartUpdateResult.Succeeded(cart);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao atualizar quantidade do item: {ex.Message}");
            return CartUpdateResult.Failed("Ocorreu um erro ao atualizar a quantidade do item.");
        }
    }

    public async Task<CheckoutResult> PrepareCheckoutAsync(string userId)
    {
        try
        {
            if (string.IsNullOrEmpty(userId))
                return CheckoutResult.Failed("ID do usuário é obrigatório.");

            var cart = await _cartRepository.GetCartAsync(userId);

            if (cart.Items.Count == 0)
                return CheckoutResult.Failed("Carrinho está vazio.");

            // Gera um ID único para o checkout
            string checkoutId = Guid.NewGuid().ToString("N");

            _logger.LogInformation($"Checkout preparado para o usuário {userId} com ID: {checkoutId}");

            return CheckoutResult.Succeeded(cart, checkoutId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao preparar checkout: {ex.Message}");
            return CheckoutResult.Failed("Ocorreu um erro ao preparar o checkout.");
        }
    }
    private CartResult ConvertToResult(Cart cart)
    {
        var cartResult = new CartResult()
        {
            Items = cart.Items,
            LastUpdated = cart.LastUpdated,
            UserId = cart.UserId
        };
        return cartResult;
    }

    public async Task<CartResult> ListCartAsync(string userId)
    {
        try
        {
            if (string.IsNullOrEmpty(userId))
                return new CartResult();

            var cart = await _cartRepository.GetCartAsync(userId);
            return ConvertToResult(cart);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao preparar checkout: {ex.Message}");
            return new CartResult();
        }
    }
}
