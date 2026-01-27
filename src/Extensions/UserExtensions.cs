using Ciandt.Retail.MCP.Models;
using Ciandt.Retail.MCP.Models.Entities;

namespace Ciandt.Retail.MCP.Extensions;

public static class UserExtensions
{
    public static CustomerProfile ToCustomerResponse(this CustomerEntity entity)
    {
        var profile = new CustomerProfile()
        {
            Id = entity.Id,
            DocumentCPF = entity.DocumentCPF,
            Email = entity.Email,
            Name = entity.Name,
            Phone = entity.Phone,
            Zip = entity.Zip,
            Gender = entity.Gender,
            Address = ToCustomerAddress(entity.Addresses)
        };

        return profile;
    }

    public static ICollection<Address> ToCustomerAddress(this ICollection<AddressEntity> addressesEntity)
    {
        ICollection<Address> addresses = new HashSet<Address>();
        foreach (var addressEntity in addressesEntity)
        {
            var address = new Address()
            {
                City = addressEntity.City,
                State = addressEntity.State,
                Street = addressEntity.Street,
                ZipCode = addressEntity.ZipCode,
                Default = addressEntity.IsDefault
            };
            addresses.Add(address);
        }
        return addresses;
    }
}
