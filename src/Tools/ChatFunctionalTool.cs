using Ciandt.Retail.MCP.Models.Result;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Reflection;

namespace Ciandt.Retail.MCP.Tools;

[McpServerToolType]
public class ChatFunctionalTool
{
    [McpServerTool(Name = "ask_tools", Title = "Customer ask about what functions are available in this service")]
    [Description("Customer ask about what functions are available in this service")]
    public async Task<CartUpdateResult> AskTools(string userId)
    {
        // Get all available tools
        var tools = GetAllMcpServerTools();

        // Process the tools list as needed
        return new CartUpdateResult
        {
            Success = true,
            Message = $"Found {tools.Count} available tools"
        };
    }

    public List<McpServerToolInfo> GetAllMcpServerTools()
    {
        var tools = new List<McpServerToolInfo>();

        // Get all assemblies in the current AppDomain
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach (var assembly in assemblies)
        {
            try
            {
                // Find all types with McpServerToolType attribute
                var toolClasses = assembly.GetTypes()
                    .Where(t => t.GetCustomAttributes(typeof(McpServerToolTypeAttribute), true).Any());

                foreach (var toolClass in toolClasses)
                {
                    // Get all methods in the class with McpServerTool attribute
                    var toolMethods = toolClass.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
                        .Where(m => m.GetCustomAttributes(typeof(McpServerToolAttribute), true).Any());

                    foreach (var method in toolMethods)
                    {
                        var attr = method.GetCustomAttribute<McpServerToolAttribute>();
                        tools.Add(new McpServerToolInfo
                        {
                            Name = attr.Name,
                            Title = attr.Title,
                            MethodName = method.Name,
                            ClassName = toolClass.Name
                        });
                    }
                }
            }
            catch (Exception)
            {
                // Some assemblies might throw exceptions when trying to load types
                // Just continue with the next assembly
                continue;
            }
        }

        return tools;
    }
}

// Class to store tool information
public class McpServerToolInfo
{
    public string Name { get; set; }
    public string Title { get; set; }
    public string MethodName { get; set; }
    public string ClassName { get; set; }
}
