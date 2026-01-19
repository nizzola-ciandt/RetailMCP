using Ciandt.Retail.MCP.Models;
using Ciandt.Retail.MCP.Models.Result;
using System.Net;

namespace Ciandt.Retail.MCP.Services;

public class DeliveryService
{
    public async Task<TrackingInformation> TrackOrderDeliveryAsync(string orderId, string trackingNumber = null)
    {
        throw new NotImplementedException();
    }

    public async Task<AddressChangeResult> RequestDeliveryAddressChangeAsync(string orderId, Address newAddress)
    {
        throw new NotImplementedException();
    }

    public async Task<RescheduleResult> RescheduleDeliveryAsync(string orderId, DateTime preferredDate, string preferredTimeSlot)
    {
        throw new NotImplementedException();
    }
}
