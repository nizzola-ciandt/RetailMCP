using Ciandt.Retail.MCP.Interfaces.Repositories;
using Ciandt.Retail.MCP.Models;
using Microsoft.Extensions.Logging;

namespace Ciandt.Retail.MCP.Repositories;

public class InvoiceRepository : IInvoiceRepository
{
    private readonly ILogger<InvoiceRepository> _logger;

    public InvoiceRepository(ILogger<InvoiceRepository> logger)
    {
        _logger = logger;
    }

    public Task<ICollection<ShippingOption>> GetShippingOptions(string userId, string zipCode)
    {
        try
        {
            _logger.LogInformation($"Getting shipping options for zip code: {zipCode}");

            // Simulação de cálculo de frete baseado no CEP
            // Em uma implementação real, isso consultaria uma API de transportadora
            var shippingOptions = CalculateShippingOptions(zipCode);

            return Task.FromResult<ICollection<ShippingOption>>(shippingOptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting shipping options");
            return Task.FromResult<ICollection<ShippingOption>>(new List<ShippingOption>());
        }
    }

    private List<ShippingOption> CalculateShippingOptions(string zipCode)
    {
        // Simulação de cálculo de frete
        // Em produção, isso seria integrado com APIs de Correios, transportadoras, etc.

        var baseShippingCost = 12.90m;
        var baseDays = 7;

        // Simula variação de custo/prazo baseado no CEP
        var firstDigit = zipCode.Length > 0 && char.IsDigit(zipCode[0])
            ? int.Parse(zipCode[0].ToString())
            : 0;

        var distanceMultiplier = 1 + (firstDigit * 0.1m);
        var daysMultiplier = firstDigit > 5 ? 1.5 : 1.0;

        return new List<ShippingOption>
        {
            new ShippingOption
            {
                Id = "pickup",
                Name = "Retirar na loja",
                Carrier = "Retirada Local",
                Cost = 0.0m,
                EstimatedDeliveryDays = 0,
                IsExpedited = false,
                Description = "Retire seu pedido diretamente em nossa loja sem custos adicionais."
            },

            new ShippingOption
            {
                Id = "standard",
                Name = "Envio Padrão",
                Carrier = "Correios",
                Cost = Math.Round(baseShippingCost * distanceMultiplier, 2),
                EstimatedDeliveryDays = (int)(baseDays * daysMultiplier),
                IsExpedited = false,
                Description = $"Entrega econômica em até {(int)(baseDays * daysMultiplier)} dias úteis."
            },

            new ShippingOption
            {
                Id = "express",
                Name = "Envio Expresso",
                Carrier = "Transportadora",
                Cost = Math.Round(25.50m * distanceMultiplier, 2),
                EstimatedDeliveryDays = (int)(3 * daysMultiplier),
                IsExpedited = true,
                Description = $"Entrega rápida em até {(int)(3 * daysMultiplier)} dias úteis."
            },

            new ShippingOption
            {
                Id = "next_day",
                Name = "Entrega 24h",
                Carrier = "Entrega Rápida",
                Cost = Math.Round(39.90m * distanceMultiplier, 2),
                EstimatedDeliveryDays = 1,
                IsExpedited = true,
                Description = "Entrega garantida no próximo dia útil.",
                IsAvailable = firstDigit <= 3 // Disponível apenas para regiões próximas
            }
        }.Where(o => o.IsAvailable).ToList();
    }
}