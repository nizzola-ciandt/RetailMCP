using Ciandt.Retail.MCP.Interfaces.Repositories;
using Ciandt.Retail.MCP.Interfaces.Services;
using Ciandt.Retail.MCP.Models;
using Ciandt.Retail.MCP.Models.Request;
using Ciandt.Retail.MCP.Models.Result;

namespace Ciandt.Retail.MCP.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ILogger _logger;

    public CustomerService(ICustomerRepository customerRepository, ILogger<CustomerService> logger)
    {
        _logger = logger;
        _customerRepository = customerRepository;
    }

    public async Task<AddressCreatedResult> AddAddressAsync(CustomerAddressCreateRequest userData)
    {
        if (userData == null)
        {
            _logger.LogWarning("AddAddressAsync called with null request data");
            return new AddressCreatedResult { Success = false, Message = "Request data cannot be null" };
        }

        if (string.IsNullOrEmpty(userData.CustomerId))
        {
            _logger.LogWarning("AddAddressAsync called with empty customer ID");
            return new AddressCreatedResult { Success = false, Message = "Customer ID is required" };
        }

        try
        {
            _logger.LogInformation($"Adding address for customer ID: {userData.CustomerId}");
            var result = await _customerRepository.AddAddressAsync(userData);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error adding address for customer ID: {userData.CustomerId}");
            return new AddressCreatedResult
            {
                Success = false,
                Message = $"Error adding address: {ex.Message}"
            };
        }
    }

    public async Task<CustomerProfileCreatedResult> CreateProfileAsync(CustomerCreateRequest request)
    {
        if (request == null)
        {
            _logger.LogWarning("CreateProfileAsync called with null request data");
            return new CustomerProfileCreatedResult
            {
                Success = false,
                Message = "Customer profile data cannot be null"
            };
        }

        try
        {
            _logger.LogInformation("Creating new customer profile");
            var result = await _customerRepository.CreateProfileAsync(request);

            if (result.Success)
            {
                _logger.LogInformation($"Successfully created customer profile with ID: {result.CustomerId}");
            }
            else
            {
                _logger.LogWarning("Failed to create customer profile");
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating customer profile");
            return new CustomerProfileCreatedResult
            {
                Success = false,
                Message = $"Error creating customer profile: {ex.Message}"
            };
        }
    }

    public async Task<CustomerProfile> GetCustomerProfileAsync(string customerId, List<string> fields = null)
    {
        if (string.IsNullOrEmpty(customerId))
        {
            _logger.LogWarning("GetCustomerProfileAsync called with empty customer ID");
            return null;
        }

        try
        {
            _logger.LogInformation($"Retrieving customer profile for ID: {customerId}");
            var customerProfile = await _customerRepository.GetCustomerProfileAsync(customerId, fields);

            if (customerProfile == null)
            {
                _logger.LogWarning($"Customer profile not found for ID: {customerId}");
            }
            else
            {
                _logger.LogInformation($"Successfully retrieved customer profile for ID: {customerId}");
            }

            return customerProfile;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving customer profile for ID: {customerId}");
            return null;
        }
    }

    public async Task<CustomerProfile?> FindCustomerProfileAsync(CustomerFindRequest customerParams)
    {
        if (customerParams == null)
        {
            _logger.LogWarning("FindCustomerProfileAsync called with null parameters");
            return null;
        }

        // Valida se pelo menos um parâmetro foi fornecido
        if (string.IsNullOrWhiteSpace(customerParams.Name) &&
            string.IsNullOrWhiteSpace(customerParams.Email) &&
            string.IsNullOrWhiteSpace(customerParams.Phone) &&
            string.IsNullOrWhiteSpace(customerParams.DocumentCPF))
        {
            _logger.LogWarning("FindCustomerProfileAsync called with no search parameters");
            return null;
        }

        try
        {
            _logger.LogInformation("Finding customer profile with provided search criteria");

            var customerProfile = await _customerRepository.FindCustomerProfileAsync(customerParams);

            if (customerProfile == null)
            {
                _logger.LogInformation("Customer profile not found with the provided criteria");
            }
            else
            {
                _logger.LogInformation($"Successfully found customer profile: ID={customerProfile.Id}, Name={customerProfile.Name}");
            }

            return customerProfile;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error finding customer profile");
            return null;
        }
    }

    public async Task<ICollection<Address>> ListAddressAsync(string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            _logger.LogWarning("ListAddressAsync called with empty user ID");
            return new List<Address>();
        }

        try
        {
            _logger.LogInformation($"Retrieving addresses for customer ID: {userId}");
            var addresses = await _customerRepository.ListAddressAsync(userId);

            _logger.LogInformation($"Retrieved {addresses.Count} addresses for customer ID: {userId}");
            return addresses;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving addresses for customer ID: {userId}");
            return new List<Address>();
        }
    }

    public async Task<CustomerProfileUpdateResult> UpdateCustomerProfileAsync(CustomerUpdateRequest updatedData)
    {
        if (updatedData == null)
        {
            _logger.LogWarning("UpdateCustomerProfileAsync called with null update data");
            return new CustomerProfileUpdateResult
            {
                Success = false,
                Message = "Update data cannot be null"
            };
        }

        if (string.IsNullOrEmpty(updatedData.CustomerId))
        {
            _logger.LogWarning("UpdateCustomerProfileAsync called with empty customer ID");
            return new CustomerProfileUpdateResult
            {
                Success = false,
                Message = "Customer ID is required"
            };
        }

        try
        {
            _logger.LogInformation($"Updating customer profile for ID: {updatedData.CustomerId}");
            var result = await _customerRepository.UpdateCustomerProfileAsync(updatedData);

            if (result.Success)
            {
                _logger.LogInformation($"Successfully updated customer profile for ID: {updatedData.CustomerId}");
            }
            else
            {
                _logger.LogWarning($"Failed to update customer profile for ID: {updatedData.CustomerId}");
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating customer profile for ID: {updatedData.CustomerId}");
            return new CustomerProfileUpdateResult
            {
                Success = false,
                Message = $"Error updating customer profile: {ex.Message}"
            };
        }
    }
}
