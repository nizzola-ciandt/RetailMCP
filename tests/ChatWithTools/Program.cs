using Azure.AI.OpenAI;
using Azure.Identity;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Client;
using ModelContextProtocol.Server;

//local
/*
var clientTransport = new StdioClientTransport(new StdioClientTransportOptions
{
    Name = "RetailMCPS",
    Command = "dotnet",
    Arguments = new[] { "run", "--project", @"..\Ciandt.Retail.MCP" }
});
*/
var mcpUrl = "http://localhost:5083/mcp";
//var mcpUrl = "https://ciandtretailmcpcoe1.livelyisland-c878f012.eastus.azurecontainerapps.io/mcp";
var clientTransport = new HttpClientTransport(
    new HttpClientTransportOptions
    {
        Name = "RetailMCP",
        Endpoint = new Uri(mcpUrl)
    });

var mcpClient = await McpClient.CreateAsync(clientTransport);

// Get available functions.
IList<McpClientTool> tools = await mcpClient.ListToolsAsync();

// Print the list of tools available from the server.
foreach (var tool in tools)
{
    Console.WriteLine($"Name: {tool.Name}");
    Console.WriteLine($"Descripton: {tool.Description}");
    Console.WriteLine($"Schema: {tool.JsonSchema}");
    Console.WriteLine(new string('-', 30));
}

// Execute a tool (this would normally be driven by LLM tool invocations).
/*
var result = await mcpClient.CallToolAsync(
    "echo",
    new Dictionary<string, object?>() { ["message"] = "Hello MCP!" },
    cancellationToken: CancellationToken.None);

// echo always returns one and only one text content object
Console.WriteLine(result.Content.First(c => c.Type == "text").ToString());
*/


// Create an IChatClient using Azure OpenAI.

var aiUrl = "https://whatsappia.openai.azure.com/";
IChatClient client =
    new ChatClientBuilder(
        new AzureOpenAIClient(new Uri(aiUrl),
        new DefaultAzureCredential())
        .GetChatClient("gpt-4o").AsIChatClient())
    .UseFunctionInvocation()
    .Build();

// Conversational loop that can utilize the tools via prompts.
List<ChatMessage> messages = [];
while (true)
{
    Console.Write("Prompt: ");
    messages.Add(new(ChatRole.User, Console.ReadLine()));

    List<ChatResponseUpdate> updates = [];
    await foreach (ChatResponseUpdate update in client
        .GetStreamingResponseAsync(messages, new() { Tools = [.. tools] }))
    {
        if (! string.IsNullOrEmpty(update.Text ))
        {
            Console.Write(update);
            updates.Add(update);
        }
    }
    Console.WriteLine();
    
    messages.AddMessages(updates);
}
