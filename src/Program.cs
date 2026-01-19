using Ciandt.Retail.MCP;
using Ciandt.Retail.MCP.Tools;

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
