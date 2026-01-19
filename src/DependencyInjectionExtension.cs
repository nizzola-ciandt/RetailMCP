using Ciandt.Retail.MCP.Interfaces;
using Ciandt.Retail.MCP.Interfaces.Repositories;
using Ciandt.Retail.MCP.Repositories;
using Ciandt.Retail.MCP.Repository;
using Ciandt.Retail.MCP.Services;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;
using Serilog.Events;
using System.Reflection;

namespace Ciandt.Retail.MCP;

public static class DependencyInjectionExtension
{
    public static TBuilder AddServiceDefaults<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        builder.AddDependencyInjection();
        builder.AddDefaultHealthChecks();
        builder.Services.AddMemoryCache();

        builder.Services.AddMcpServer()
                .WithHttpTransport(o => o.Stateless = true)
                .WithToolsFromAssembly();
        
        return builder;
    }

    public static void AddDependencyInjection<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        builder.AddLogger();
        builder.AddDependencyInjectionWithTag("Service");
        //posteriormente trocar por : builder.AddDependencyInjectionWithTag("Repository");
        builder.AddFakeRepositories();
    }

    public static void AddLogger<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog(Log.Logger);
    }

    public static void AddFakeRepositories<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        builder.Services.AddMemoryCache();
        builder.Services.AddScoped<IProductRepository, ProductFakeRepository>();
        builder.Services.AddSingleton<ICartRepository, InMemoryCartRepository>();
        builder.Services.AddSingleton<IPaymentRepository, PaymentFakeRepository>();
        builder.Services.AddSingleton<IInvoiceRepository, InvoiceFakeRepository>();
        builder.Services.AddSingleton<ICustomerRepository, CustomerFakeRepository>();
    }

    private static void AddDependencyInjectionWithTag<TBuilder>(this TBuilder builder, string tag) where TBuilder : IHostApplicationBuilder
    {
        var types = Assembly.GetExecutingAssembly().GetTypes()
       .Where(x => x.GetInterfaces().Any(i => i.Name.EndsWith(tag)));

        foreach (var type in types)
        {
            var interfaces = type.GetInterfaces();
            foreach (var inter in interfaces)
            {
                builder.Services.AddScoped(inter, type);
            }
        }
    }

    public static void AddDefaultHealthChecks<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        builder.Services.AddHealthChecks()
            // Add a default liveness check to ensure app is responsive
            .AddCheck("self", () => HealthCheckResult.Healthy(), ["live"]);
    }

    public static WebApplication MapDefaultEndpoints(this WebApplication app)
    {
        // Adding health checks endpoints to applications in non-development environments has security implications.
        // See https://aka.ms/dotnet/aspire/healthchecks for details before enabling these endpoints in non-development environments.
        if (app.Environment.IsDevelopment())
        {
            // All health checks must pass for app to be considered ready to accept traffic after starting
            app.MapHealthChecks("/health");

            // Only health checks tagged with the "live" tag must pass for app to be considered alive
            app.MapHealthChecks("/alive", new HealthCheckOptions
            {
                Predicate = r => r.Tags.Contains("live")
            });
        }
        app.MapMcp("/mcp");

        return app;
    }
}
