using Ciandt.Retail.MCP.Models;
using Ciandt.Retail.MCP.Models.Result;
using System.Net;

namespace Ciandt.Retail.MCP.Services;

public class DeliveryService
{
    public async Task<TrackingInformation> TrackOrderDeliveryAsync(int orderId, string trackingNumber = null)
    {
        throw new NotImplementedException();
    }

    public async Task<AddressChangeResult> RequestDeliveryAddressChangeAsync(int orderId, Address newAddress)
    {
        throw new NotImplementedException();
    }

    public async Task<RescheduleResult> RescheduleDeliveryAsync(int orderId, DateTime preferredDate, string preferredTimeSlot)
    {
        throw new NotImplementedException();
    }
}
