using Ciandt.Retail.MCP.Data;
using Ciandt.Retail.MCP.Interfaces.Repositories;
using Ciandt.Retail.MCP.Models;
using Ciandt.Retail.MCP.Models.Entities;
using Ciandt.Retail.MCP.Models.Request;
using Ciandt.Retail.MCP.Models.Result;
using Microsoft.EntityFrameworkCore;

namespace Ciandt.Retail.MCP.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly RetailDbContext _context;
    private readonly ILogger<CustomerRepository> _logger;

    public CustomerRepository(RetailDbContext context, ILogger<CustomerRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<CustomerProfile> GetCustomerProfileAsync(string customerId, List<string>? fields = null)
    {
        try
        {
            if (string.IsNullOrEmpty(customerId) || !int.TryParse(customerId, out var id))
            {
                _logger.LogWarning("Invalid customer ID");
                return null!;
            }

            var customer = await _context.Customers
                .Include(c => c.Addresses)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (customer == null)
            {
                _logger.LogWarning($"Customer not found: {customerId}");
                return null!;
            }

            var profile = new CustomerProfile
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                Phone = customer.Phone,
                Zip = customer.Zip,
                Address = customer.Addresses.Select(a => new Address
                {
                    Street = a.Street,
                    City = a.City,
                    State = a.State,
                    ZipCode = a.ZipCode,
                    Default = a.IsDefault
                }).ToList()
            };

            return profile;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting customer profile: {customerId}");
            return null!;
        }
    }

    public async Task<CustomerProfileUpdateResult> UpdateCustomerProfileAsync(CustomerUpdateRequest updatedData)
    {
        try
        {
            if (string.IsNullOrEmpty(updatedData.CustomerId) || !int.TryParse(updatedData.CustomerId, out var id))
            {
                return new CustomerProfileUpdateResult
                {
                    Success = false,
                    Message = "Invalid customer ID"
                };
            }

            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return new CustomerProfileUpdateResult
                {
                    Success = false,
                    Message = "Customer not found"
                };
            }

            if (!string.IsNullOrEmpty(updatedData.Name)) customer.Name = updatedData.Name;
            if (!string.IsNullOrEmpty(updatedData.Email)) customer.Email = updatedData.Email;
            if (!string.IsNullOrEmpty(updatedData.Phone)) customer.Phone = updatedData.Phone;
            if (!string.IsNullOrEmpty(updatedData.Zip)) customer.Zip = updatedData.Zip;

            customer.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new CustomerProfileUpdateResult
            {
                Success = true,
                Message = "Customer profile updated successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating customer profile");
            return new CustomerProfileUpdateResult
            {
                Success = false,
                Message = $"Error: {ex.Message}"
            };
        }
    }

    public async Task<CustomerProfileCreatedResult> CreateProfileAsync(CustomerCreateRequest request)
    {
        try
        {
            var newCustomer = new CustomerEntity
            {
                Name = request.Name ?? string.Empty,
                Email = request.Email ?? string.Empty,
                Phone = request.Phone ?? string.Empty,
                Zip = request.Zip ?? string.Empty,
                CreatedAt = DateTime.UtcNow
            };

            _context.Customers.Add(newCustomer);
            await _context.SaveChangesAsync();

            return new CustomerProfileCreatedResult
            {
                Success = true,
                Message = "Customer profile created successfully",
                CustomerId = newCustomer.Id.ToString()
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating customer profile");
            return new CustomerProfileCreatedResult
            {
                Success = false,
                Message = $"Error: {ex.Message}"
            };
        }
    }

    public async Task<ICollection<Address>> ListAddressAsync(string userId)
    {
        try
        {
            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var id))
            {
                return new List<Address>();
            }

            var addresses = await _context.Addresses
                .Where(a => a.CustomerId == id)
                .Select(a => new Address
                {
                    Street = a.Street,
                    City = a.City,
                    State = a.State,
                    ZipCode = a.ZipCode,
                    Default = a.IsDefault
                })
                .ToListAsync();

            return addresses;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error listing addresses: {userId}");
            return new List<Address>();
        }
    }

    public async Task<AddressCreatedResult> AddAddressAsync(CustomerAddressCreateRequest userData)
    {
        try
        {
            if (string.IsNullOrEmpty(userData.CustomerId) || !int.TryParse(userData.CustomerId, out var id))
            {
                return new AddressCreatedResult
                {
                    Success = false,
                    Message = "Invalid customer ID"
                };
            }

            var customer = await _context.Customers
                .Include(c => c.Addresses)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (customer == null)
            {
                return new AddressCreatedResult
                {
                    Success = false,
                    Message = "Customer not found"
                };
            }

            // Se o novo endereço é default, remove o default dos outros
            if (userData.Default)
            {
                foreach (var addr in customer.Addresses)
                {
                    addr.IsDefault = false;
                }
            }

            var newAddress = new AddressEntity
            {
                CustomerId = id,
                Street = userData.Street ?? string.Empty,
                City = userData.City ?? string.Empty,
                State = userData.State ?? string.Empty,
                ZipCode = userData.ZipCode ?? string.Empty,
                IsDefault = userData.Default,
                CreatedAt = DateTime.UtcNow
            };

            _context.Addresses.Add(newAddress);
            await _context.SaveChangesAsync();

            return new AddressCreatedResult
            {
                Success = true,
                Message = "Address added successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding address");
            return new AddressCreatedResult
            {
                Success = false,
                Message = $"Error: {ex.Message}"
            };
        }
    }
}