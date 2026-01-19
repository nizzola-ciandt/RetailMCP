using Ciandt.Retail.MCP.Interfaces.Repositories;
using Ciandt.Retail.MCP.Models;

namespace Ciandt.Retail.MCP.Repositories;

public class InvoiceFakeRepository : IInvoiceRepository
{
    public Task<ICollection<ShippingOption>> GetShippingOptions(string userId, string zipCode)
    {
        var shippingOptions = new List<ShippingOption>
        {
            new ShippingOption
            {
                Id = "pickup",
                Name = "Retirar na loja",
                Carrier = "Retirada Local",
                Cost = 0.0m,  // Sem custo
                EstimatedDeliveryDays = 0,  // Disponível para retirada imediata
                IsExpedited = false,
                Description = "Retire seu pedido diretamente em nossa loja sem custos adicionais."
            },
            
            new ShippingOption
            {
                Id = "standard",
                Name = "Envio Padrão",
                Carrier = "Correios",
                Cost = 12.90m,
                EstimatedDeliveryDays = 7,
                IsExpedited = false,
                Description = "Entrega econômica em até 7 dias úteis."
            },
            
            new ShippingOption
            {
                Id = "express",
                Name = "Envio Expresso",
                Carrier = "Transportadora",
                Cost = 25.50m,
                EstimatedDeliveryDays = 3,
                IsExpedited = true,
                Description = "Entrega rápida em até 3 dias úteis."
            },
            
            new ShippingOption
            {
                Id = "next_day",
                Name = "Entrega 24h",
                Carrier = "Entrega Rápida",
                Cost = 39.90m,
                EstimatedDeliveryDays = 1,
                IsExpedited = true,
                Description = "Entrega garantida no próximo dia útil."
            }
        };

        return Task.FromResult<ICollection<ShippingOption>>(shippingOptions);
    }
}