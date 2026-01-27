using Ciandt.Retail.MCP.Models;
using Ciandt.Retail.MCP.Models.Request;
using Ciandt.Retail.MCP.Models.Result;

namespace Ciandt.Retail.MCP.Interfaces.Services;
public interface ICustomerService
{
    Task<CustomerProfile> GetCustomerProfileAsync(string customerId, List<string> fields = null);
    Task<CustomerProfileUpdateResult> UpdateCustomerProfileAsync(CustomerUpdateRequest updatedData);
    Task<CustomerProfileCreatedResult> CreateProfileAsync(CustomerCreateRequest request);
    Task<ICollection<Address>> ListAddressAsync(string userId);
    Task<AddressCreatedResult> AddAddressAsync(CustomerAddressCreateRequest userData);
    Task<CustomerProfile?> FindCustomerProfileAsync(CustomerFindRequest customerParams);
}