
using System.ComponentModel;
using System.Text.Json;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol;

// 1. Configure Semantic Kernel and OpenAI
var apiKey = "sk-proj-zXOsA";

if (string.IsNullOrWhiteSpace(apiKey))
{
    throw new InvalidOperationException(
        "Set the OPENAI_API_KEY environment variable first.");
}

var builder = Kernel.CreateBuilder();

builder.AddOpenAIChatCompletion(
    modelId: "gpt-4.1-mini",
    apiKey: apiKey);

var kernel = builder.Build();

Console.WriteLine("Connecting MCP Gateway at 5000 local host....");
// 2. Connect to the MCP Gateway
var transport = new HttpClientTransport(
    new HttpClientTransportOptions
    {
        Endpoint = new Uri("http://localhost:5000/mcp")
    });

await using var mcpClient = await McpClient.CreateAsync(transport);
Console.WriteLine("Connection Establish....");
// 3. Discover gateway tools
var mcpTools = await mcpClient.ListToolsAsync();



Console.WriteLine("Semantic Kernel initialized.");
Console.WriteLine($"Gateway tools discovered: {mcpTools.Count}");

foreach (var tool in mcpTools)
{
    Console.WriteLine($"- {tool.Name}: {tool.Description}");
}

// 4. Discover the actual HR and IT backend tool catalog
var catalogResult = await mcpClient.CallToolAsync(
    "list_backend_tools",
    new Dictionary<string, object?>());

var toolCatalog = string.Join(
    "\n",
    catalogResult.Content
        .OfType<TextContentBlock>()
        .Select(x => x.Text));

Console.WriteLine("\nBackend tool catalog:");
Console.WriteLine(toolCatalog);

// 5. Register one Semantic Kernel function for gateway execution
kernel.Plugins.AddFromObject(
    new GatewayPlugin(mcpClient),
    "EmployeeGateway");

Console.WriteLine("\nTools are ready for Semantic Kernel.");

// 6. Ask a question; Semantic Kernel handles automatic function calling
var settings = new OpenAIPromptExecutionSettings
{
    FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
};

Console.WriteLine("\nAsk an employee question:");
var question = Console.ReadLine();

if (!string.IsNullOrWhiteSpace(question))
{
    var prompt = $"""
        You are a read-only employee assistant.

        Use EmployeeGateway.call_backend_tool to retrieve information.
        Select the exact server and tool name from this backend catalog.
        Pass argumentsJson as a valid JSON string.
        Never invent tool results. If no suitable tool exists, say so.

        Backend tool catalog:
        {toolCatalog}

        Employee question: {question}
        """;

    var response = await kernel.InvokePromptAsync(
        prompt,
        new KernelArguments(settings));

    Console.WriteLine("\nAssistant:");
    Console.WriteLine(response);
}

// Local plugin: forwards the LLM's tool selection to the MCP Gateway.
public sealed class GatewayPlugin(McpClient client)
{
    [KernelFunction("call_backend_tool")]
    [Description(
        "Call an HR or IT backend tool through the MCP Gateway. " +
        "Use the exact server, tool name, and arguments from the catalog.")]
    public async Task<string> CallBackendTool(
        [Description("Backend server: hr or it")] string server,
        [Description("Exact backend tool name")] string toolName,
        [Description("Backend arguments serialized as a JSON string")]
        string argumentsJson)
    {
        var arguments =
            JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(
                argumentsJson) ?? new();

        var result = await client.CallToolAsync(
            "call_backend_tool",
            new Dictionary<string, object?>
            {
                ["server"] = server,
                ["toolName"] = toolName,
                ["arguments"] = arguments
            });

        return string.Join(
            "\n",
            result.Content
                .OfType<TextContentBlock>()
                .Select(x => x.Text));
    }
}
