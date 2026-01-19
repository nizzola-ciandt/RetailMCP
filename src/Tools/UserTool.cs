using Ciandt.Retail.MCP.Interfaces.Services;
using Ciandt.Retail.MCP.Models;
using Ciandt.Retail.MCP.Models.Request;
using Ciandt.Retail.MCP.Models.Result;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace Ciandt.Retail.MCP.Tools;

[McpServerToolType]

public class UserTool
{
    private readonly ILogger _logger;
    private readonly ICustomerService _customerService;

    public UserTool(ILogger<UserTool> logger, ICustomerService customerService)
    {
        _logger = logger;
        _customerService = customerService;
    }

    [McpServerTool(Name = "account_management", Title = "Customer needs help check information about their account (login, password, registration data)")]
    [Description("Provides account information or helps update account details.")]
    public async Task<CustomerProfile> GetAccountInformationAsync(string userId)
    {
        _logger.LogInformation($"Retrieving account information for user {userId}");
        return await _customerService.GetCustomerProfileAsync(userId);
    }

    [McpServerTool(Name = "account_update", Title = "Customer needs to update your account data")]
    [Description("Update data from user account.")]
    public async Task<CustomerProfileUpdateResult> UpdateAccountInformationAsync(CustomerUpdateRequest userData)
    {
        _logger.LogInformation($"Updating data of client account ");
        return await _customerService.UpdateCustomerProfileAsync(userData);
    }

    [McpServerTool(Name = "account_create", Title = "Customer needs to create your new account")]
    [Description("Customer needs to create your new account")]
    public async Task<CustomerProfileCreatedResult> UpdateAccountInformationAsync(CustomerCreateRequest userData)
    {
        _logger.LogInformation($"Updating data of client account ");
        return await _customerService.CreateProfileAsync(userData);
    }

    [McpServerTool(Name = "account_addresses", Title = "Customer needs check what are your available addresses to ship")]
    [Description("Customer needs check what are your available addresses to ship")]
    public async Task<ICollection<Address>> ListAddressInformationAsync(string userId)
    {
        _logger.LogInformation($"Updating data of client account ");
        return await _customerService.ListAddressAsync(userId);
    }

    [McpServerTool(Name = "account_add_address", Title = "Customer needs to add new address to your profile")]
    [Description("Customer needs to add new address to your profile")]
    public async Task<AddressCreatedResult> AddAddressInformationAsync(CustomerAddressCreateRequest userData)
    {
        _logger.LogInformation($"Updating data of client account ");
        return await _customerService.AddAddressAsync(userData);
    }
}
