using Ciandt.Retail.MCP;
using Ciandt.Retail.MCP.Data;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.AddDependencyInjection();
builder.Logging.AddConsole(o => o.LogToStandardErrorThreshold = LogLevel.Information);

// Add service defaults (Database, DI, Logging, MCP Server)
builder.AddServiceDefaults();

// 1. Register the MCP server in the service container
builder.Services
    .AddMcpServer()
    //.WithStdioServerTransport() // Use Stdio transport for the MCP server
    .WithHttpTransport(o => o.Stateless = true)           // Use HTTP transport for the MCP server
    .WithToolsFromAssembly();      // Automatically discoer and register MCP tools from this assembly

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Add HTTP Client for external services
builder.Services.AddHttpClient();

var app = builder.Build();

// Apply migrations automatically in development
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<RetailDbContext>();
    dbContext.Database.EnsureCreated(); // Ou use dbContext.Database.Migrate() para migrations
}

app.UseHttpsRedirection();

// Map default endpoints (Health checks and MCP)
app.MapDefaultEndpoints();

// Map root route
app.MapGet("/", () => "Welcome to RetailMCP with Entity Framework Core!");

app.Run();

Console.WriteLine("RetailMCP Server Stopped");

/*
var builder = WebApplication.CreateBuilder(args);

builder.AddDependencyInjection();
builder.Logging.AddConsole(o => o.LogToStandardErrorThreshold = LogLevel.Information);

// 1. Register the MCP server in the service container
builder.Services
    .AddMcpServer()
    //.WithStdioServerTransport() // Use Stdio transport for the MCP server
    .WithHttpTransport(o => o.Stateless = true)           // Use HTTP transport for the MCP server
    .WithToolsFromAssembly();      // Automatically discoer and register MCP tools from this assembly

builder.Services.AddHttpClient();

var app = builder.Build();
app.UseHttpsRedirection();
// 2. Map root route to a welcome message
app.MapGet("/", () => "Welcome to .NET MCP Server !!");
// 3. Map the "/mcp" endpoint to the MCP server
app.MapMcp("/mcp");
// 4. Start the server


app.Run();

Console.WriteLine("Finish MCP Server");
*/