using Ciandt.Retail.MCP.Data;
using Ciandt.Retail.MCP.Endpoints;
using Ciandt.Retail.MCP.Interfaces.Repositories;
using Ciandt.Retail.MCP.Repositories;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
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

        builder.Services.AddMcpServer()
                .WithHttpTransport(o => o.Stateless = true)
                .WithToolsFromAssembly();

        return builder;
    }

    public static void AddDependencyInjection<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        builder.AddLogger();
        builder.AddDatabase();
        builder.AddDependencyInjectionWithTag("Service");
        builder.AddRepositories();
    }

    public static void AddLogger<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog(Log.Logger);
    }

    public static void AddDatabase<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        builder.Services.AddDbContext<RetailDbContext>(options =>
        {
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);
                sqlOptions.CommandTimeout(60);
            });

            // Enable sensitive data logging only in development
            if (builder.Environment.IsDevelopment())
            {
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            }
        });

        // Add health check for database
        //builder.Services.AddHealthChecks()
        //    .AddDbContextCheck<RetailDbContext>("database", tags: new[] { "ready", "db" });
    }

    public static void AddRepositories<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        // Register all repositories with real implementations
        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        builder.Services.AddScoped<ICartRepository, CartRepository>();
        builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
        builder.Services.AddScoped<IOrderRepository, OrderRepository>();
        builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
        builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
        builder.Services.AddScoped<IPromotionRepository, PromotionRepository>();
    }

    // Kept for backward compatibility - use AddRepositories instead
    //[Obsolete("Use AddRepositories instead. This method is kept for backward compatibility.")]
    //public static void AddFakeRepositories<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    //{
    //    builder.Services.AddMemoryCache();
    //    builder.Services.AddScoped<IProductRepository, ProductFakeRepository>();
    //    builder.Services.AddSingleton<ICartRepository, InMemoryCartRepository>();
    //    builder.Services.AddSingleton<IPaymentRepository, PaymentFakeRepository>();
    //    builder.Services.AddSingleton<IInvoiceRepository, InvoiceFakeRepository>();
    //    builder.Services.AddSingleton<ICustomerRepository, CustomerFakeRepository>();
    //}

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

        app.MapUserEndpoints();

        return app;
    }
}