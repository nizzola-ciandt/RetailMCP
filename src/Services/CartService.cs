using Ciandt.Retail.MCP.Interfaces.Repositories;
using Ciandt.Retail.MCP.Interfaces.Services;
using Ciandt.Retail.MCP.Models;
using Ciandt.Retail.MCP.Models.Result;

namespace Ciandt.Retail.MCP.Services;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<CartService> _logger;

    public CartService(
        IProductRepository productsRepository,
        ICartRepository cartRepository,
        IOrderRepository orderRepository,
        ILogger<CartService> logger)
    {
        _cartRepository = cartRepository;
        _productRepository = productsRepository;
        _orderRepository = orderRepository;
        _logger = logger;
    }

    public async Task<CartUpdateResult> AddItemAsync(string userId, int productId, int quantity)
    {
        try
        {
            if (string.IsNullOrEmpty(userId))
                return CartUpdateResult.Failed("ID do usuário é obrigatório.");

            if (productId==0)
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

    public async Task<CartUpdateResult> UpdateItemQuantityAsync(string userId, int productId, int newQuantity)
    {
        try
        {
            if (string.IsNullOrEmpty(userId))
                return CartUpdateResult.Failed("ID do usuário é obrigatório.");

            if (productId==0)
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

            return await CreateOrderAsync(userId, cart);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao preparar checkout: {ex.Message}");
            return CheckoutResult.Failed("Ocorreu um erro ao preparar o checkout.");
        }
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
            _logger.LogError(ex, $"Erro ao listar carrinho: {ex.Message}");
            return new CartResult();
        }
    }

    #region Private Methods

    private async Task<CheckoutResult> CreateOrderAsync(string userId, Cart cart)
    {
        try
        {
            // Valida se o userId é um ID numérico válido
            if (!int.TryParse(userId, out int customerId))
            {
                _logger.LogError($"Invalid userId format: {userId}");
                return CheckoutResult.Failed("ID do usuário inválido.");
            }

            // Calcula valores do pedido
            var subtotal = cart.Items.Sum(item => item.Price * item.Quantity);

            // Frete padrão (pode ser parametrizado ou calculado dinamicamente)
            var shippingCost = CalculateShippingCost(cart, subtotal);

            var totalAmount = subtotal + shippingCost;

            _logger.LogInformation($"Creating order for customer {customerId}: Subtotal={subtotal:C}, Shipping={shippingCost:C}, Total={totalAmount:C}");

            // Cria o pedido no banco de dados
            var orderId = await _orderRepository.CreateOrderAsync(
                customerId: customerId,
                items: cart.Items,
                shippingCost: shippingCost,
                shippingMethod: "Standard", // Pode ser parametrizado
                shippingZipCode: null // Pode vir de um endereço padrão do cliente
            );

            _logger.LogInformation($"Order created successfully: OrderId={orderId}");

            // Limpa o carrinho após criar o pedido
            await _cartRepository.DeleteCartAsync(userId);
            _logger.LogInformation($"Cart cleared for user {userId} after order creation");

            // Retorna resultado de sucesso com informações do pedido
            return CheckoutResult.Succeeded(
                orderId: orderId,
                subtotal: subtotal,
                shippingCost: shippingCost,
                totalAmount: totalAmount,
                items: cart.Items.ToList()
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error creating order for user {userId}: {ex.Message}");
            return CheckoutResult.Failed($"Erro ao criar o pedido: {ex.Message}");
        }
    }

    private decimal CalculateShippingCost(Cart cart, decimal subtotal)
    {
        // Regras de negócio para cálculo de frete
        // 1. Frete grátis para compras acima de R$ 200
        if (subtotal >= 200m)
        {
            _logger.LogInformation("Free shipping applied (order > R$ 200)");
            return 0m;
        }

        // 2. Frete fixo baseado na quantidade de itens
        var totalItems = cart.Items.Sum(i => i.Quantity);

        if (totalItems <= 3)
        {
            return 15m; // Frete para até 3 itens
        }
        else if (totalItems <= 6)
        {
            return 25m; // Frete para até 6 itens
        }
        else
        {
            return 35m; // Frete para mais de 6 itens
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

    #endregion
}