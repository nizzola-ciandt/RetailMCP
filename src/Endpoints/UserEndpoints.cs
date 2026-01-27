using Ciandt.Retail.MCP.Data;
using Ciandt.Retail.MCP.Extensions;
using Ciandt.Retail.MCP.Models;
using Ciandt.Retail.MCP.Models.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Ciandt.Retail.MCP.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/User").WithTags(nameof(CustomerProfile));

        group.MapGet("/", async (RetailDbContext db) =>
        {
            return await db.Customers.Include(a => a.Addresses).ToListAsync();
        })
        .WithName("GetAllUsers");
        //.WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<CustomerProfile>, NotFound>> (string id, RetailDbContext db) =>
        {
            var result = await db.Customers.AsNoTracking()
                .Include(a => a.Addresses)
                .FirstOrDefaultAsync(model => model.Phone == id);

            if (result is null)
                return TypedResults.NotFound();

            var profile = result.ToCustomerResponse();

            return TypedResults.Ok(profile);

        })
        .WithName("GetUserById");
        //.WithOpenApi();

        /*
        group.MapPost("/", async (User user, MongoDbContext db) =>
        {
            db.Users.Add(user);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/User/{user.Id}",user);
        })
        .WithName("CreateUser")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (string id, User user, MongoDbContext db) =>
        {
            // Converter string para ObjectId
            if (!ObjectId.TryParse(id, out var objectId))
            {
                return TypedResults.NotFound();
            }

            // Encontrar o usuário existente
            var existingUser = await db.Users.FindAsync(objectId);
            if (existingUser == null)
            {
                return TypedResults.NotFound();
            }

            // Atualizar as propriedades (mas manter o ID original)
            existingUser.TenantId = user.TenantId;
            existingUser.UserId = user.UserId;
            existingUser.Name = user.Name;
            existingUser.Gender = user.Gender;
            existingUser.Email = user.Email;
            existingUser.MobilePhone = user.MobilePhone;
            existingUser.SecondaryPhone = user.SecondaryPhone;
            existingUser.Zip = user.Zip;
            existingUser.DocumentNumber = user.DocumentNumber;

            // Salvar as mudanças
            await db.SaveChangesAsync();

            return TypedResults.Ok();
        })
        .WithName("UpdateUser")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (string id, MongoDbContext db) =>
        {
            // Converter string para ObjectId
            if (!ObjectId.TryParse(id, out var objectId))
            {
                return TypedResults.NotFound();
            }

            // Encontrar o usuário para excluir
            var user = await db.Users.FindAsync(objectId);
            if (user == null)
            {
                return TypedResults.NotFound();
            }

            // Remover o usuário
            db.Users.Remove(user);
            await db.SaveChangesAsync();

            return TypedResults.Ok();
        })
        .WithName("DeleteUser")
        .WithOpenApi();
        */
    }
}
