using Ciandt.Retail.MCP.Interfaces.Repositories;
using Ciandt.Retail.MCP.Models;
using Ciandt.Retail.MCP.Models.Request;
using Ciandt.Retail.MCP.Models.Result;
using Microsoft.Extensions.Caching.Memory;

namespace Ciandt.Retail.MCP.Repositories;

public class CustomerFakeRepository : ICustomerRepository
{
    private readonly IMemoryCache _cache;
    private readonly ILogger _logger;
    private readonly TimeSpan _cacheExpirationTime = TimeSpan.FromHours(24);
    private const string CACHE_KEY_PREFIX = "Customer_";
    private int _lastCustomerId = 2; // Start with 2 since we have 2 fake customers

    public CustomerFakeRepository(IMemoryCache cache, ILogger<CustomerFakeRepository> logger)
    {
        _cache = cache;
        _logger = logger;
        InitializeCache();
    }

    private void InitializeCache()
    {
        // Create first fake customer
        var customer1 = new CustomerProfile
        {
            Id = 1,
            Name = "Marcio Nizzola",
            Email = "marcio.nizzola@ciandt.com",
            Phone = "5511984701979",
            Zip = "13.328-283",
            Address = new List<Address>
            {
                new Address
                {
                    Street = "123 Main St",
                    City = "New York",
                    State = "NY",
                    ZipCode = "12345",
                    Default = true
                }
            }
        };

        // Create second fake customer
        var customer2 = new CustomerProfile
        {
            Id = 2,
            Name = "Ricardo Odorczyk",
            Email = "ricardoso@ciandt.com",
            Phone = "5541999089809",
            Zip = "67890",
            Address = new List<Address>
            {
                new Address
                {
                    Street = "456 Broadway",
                    City = "Los Angeles",
                    State = "CA",
                    ZipCode = "67890",
                    Default = true
                },
                new Address
                {
                    Street = "789 Park Ave",
                    City = "Los Angeles",
                    State = "CA",
                    ZipCode = "67891",
                    Default = false
                }
            }
        };

        // Add customers to cache
        _cache.Set(GetCacheKey("1"), customer1, _cacheExpirationTime);
        _cache.Set(GetCacheKey("2"), customer2, _cacheExpirationTime);
    }

    private string GetCacheKey(string customerId) => $"{CACHE_KEY_PREFIX}{customerId}";

    public async Task<CustomerProfile> GetCustomerProfileAsync(string customerId, List<string> fields = null)
    {
        if (string.IsNullOrEmpty(customerId))
        {
            _logger.LogWarning("GetCustomerProfileAsync called with null or empty customerId");
            return null;
        }

        if (_cache.TryGetValue(GetCacheKey(customerId), out CustomerProfile customer))
        {
            _logger.LogInformation($"Customer profile found in cache for ID: {customerId}");

            if (fields != null && fields.Any())
            {
                // Create a filtered profile with only the requested fields
                var filteredProfile = new CustomerProfile { Id = customer.Id };

                foreach (var field in fields)
                {
                    switch (field.ToLowerInvariant())
                    {
                        case "name": filteredProfile.Name = customer.Name; break;
                        case "email": filteredProfile.Email = customer.Email; break;
                        case "phone": filteredProfile.Phone = customer.Phone; break;
                        case "zip": filteredProfile.Zip = customer.Zip; break;
                        case "address": filteredProfile.Address = customer.Address; break;
                    }
                }

                return filteredProfile;
            }

            return customer;
        }

        _logger.LogWarning($"Customer profile not found for ID: {customerId}");
        return null;
    }

    public async Task<CustomerProfileUpdateResult> UpdateCustomerProfileAsync(CustomerUpdateRequest updatedData)
    {
        if (updatedData == null || string.IsNullOrEmpty(updatedData.CustomerId))
        {
            _logger.LogWarning("UpdateCustomerProfileAsync called with null or invalid data");
            return new CustomerProfileUpdateResult { Success = false, Message = "Invalid customer data" };
        }

        var customerId = updatedData.CustomerId;

        if (_cache.TryGetValue(GetCacheKey(customerId), out CustomerProfile customer))
        {
            // Update customer properties if provided in the request
            if (!string.IsNullOrEmpty(updatedData.Name)) customer.Name = updatedData.Name;
            if (!string.IsNullOrEmpty(updatedData.Email)) customer.Email = updatedData.Email;
            if (!string.IsNullOrEmpty(updatedData.Phone)) customer.Phone = updatedData.Phone;
            if (!string.IsNullOrEmpty(updatedData.Zip)) customer.Zip = updatedData.Zip;

            // Update cache with the modified customer
            _cache.Set(GetCacheKey(customerId), customer, _cacheExpirationTime);

            _logger.LogInformation($"Customer profile updated for ID: {customerId}");
            return new CustomerProfileUpdateResult { Success = true, Message = "Customer profile updated successfully" };
        }

        _logger.LogWarning($"Customer profile not found for update, ID: {customerId}");
        return new CustomerProfileUpdateResult { Success = false, Message = "Customer profile not found" };
    }

    public async Task<CustomerProfileCreatedResult> CreateProfileAsync(CustomerCreateRequest request)
    {
        if (request == null)
        {
            _logger.LogWarning("CreateProfileAsync called with null request");
            return new CustomerProfileCreatedResult { Success = false, Message = "Invalid customer data" };
        }

        var newCustomerId = Interlocked.Increment(ref _lastCustomerId);
        var customerId = newCustomerId.ToString();

        var newCustomer = new CustomerProfile
        {
            Id = newCustomerId,
            Name = request.Name ?? string.Empty,
            Email = request.Email ?? string.Empty,
            Phone = request.Phone ?? string.Empty,
            Zip = request.Zip ?? string.Empty,
            Address = new List<Address>()
        };

        // Add the new customer to cache
        _cache.Set(GetCacheKey(customerId), newCustomer, _cacheExpirationTime);

        _logger.LogInformation($"New customer profile created with ID: {customerId}");
        return new CustomerProfileCreatedResult
        {
            Success = true,
            Message = "Customer profile created successfully",
            CustomerId = customerId
        };
    }

    public async Task<ICollection<Address>> ListAddressAsync(string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            _logger.LogWarning("ListAddressAsync called with null or empty userId");
            return new List<Address>();
        }

        if (_cache.TryGetValue(GetCacheKey(userId), out CustomerProfile customer))
        {
            _logger.LogInformation($"Address list retrieved for customer ID: {userId}");
            return customer.Address?.ToList() ?? new List<Address>();
        }

        _logger.LogWarning($"Customer profile not found for listing addresses, ID: {userId}");
        return new List<Address>();
    }

    public async Task<AddressCreatedResult> AddAddressAsync(CustomerAddressCreateRequest userData)
    {
        if (userData == null || string.IsNullOrEmpty(userData.CustomerId))
        {
            _logger.LogWarning("AddAddressAsync called with null or invalid data");
            return new AddressCreatedResult { Success = false, Message = "Invalid address data" };
        }

        var customerId = userData.CustomerId;

        if (_cache.TryGetValue(GetCacheKey(customerId), out CustomerProfile customer))
        {
            var newAddress = new Address
            {
                Street = userData.Street ?? string.Empty,
                City = userData.City ?? string.Empty,
                State = userData.State ?? string.Empty,
                ZipCode = userData.ZipCode ?? string.Empty,
                Default = userData.Default
            };

            // If the new address is set as default, update other addresses
            if (newAddress.Default)
            {
                foreach (var address in customer.Address)
                {
                    address.Default = false;
                }
            }

            customer.Address.Add(newAddress);

            // Update cache with the modified customer
            _cache.Set(GetCacheKey(customerId), customer, _cacheExpirationTime);

            _logger.LogInformation($"Address added to customer ID: {customerId}");
            return new AddressCreatedResult { Success = true, Message = "Address added successfully" };
        }

        _logger.LogWarning($"Customer profile not found for adding address, ID: {customerId}");
        return new AddressCreatedResult { Success = false, Message = "Customer profile not found" };
    }
}