
using System.ComponentModel;
using System.Text.Json;
using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;

var builder = WebApplication.CreateBuilder(args);

var hrUrl = "http://localhost:5170/mcp";
var itUrl = "http://localhost:5226/mcp";

// Create clients for the existing MCP servers.
var hrClient = await McpClient.CreateAsync(
    new HttpClientTransport(
        new HttpClientTransportOptions
        {
            Endpoint = new Uri(hrUrl)
        }));

var itClient = await McpClient.CreateAsync(
    new HttpClientTransport(
        new HttpClientTransportOptions
        {
            Endpoint = new Uri(itUrl)
        }));

builder.Services.AddSingleton(
    new BackendClients(hrClient, itClient));

builder.Services
    .AddMcpServer()
    .WithHttpTransport()
    .WithTools<GatewayTools>();

var app = builder.Build();

app.MapMcp("/mcp");

app.Run("http://localhost:5000");


// Holds the connections to the backend MCP servers.
public sealed class BackendClients(
    McpClient hr,
    McpClient it)
{
    public McpClient Get(string server) =>
        server.ToLowerInvariant() switch
        {
            "hr" => hr,
            "it" => it,
            _ => throw new ArgumentException(
                "Server must be 'hr' or 'it'.")
        };

    public async Task<string> DiscoverAsync()
    {
        var catalog = new List<object>();

        foreach (var server in new[] { "hr", "it" })
        {
            var tools = await Get(server).ListToolsAsync();

            catalog.AddRange(tools.Select(tool => new
            {
                server,
                tool.Name,
                tool.Description,
                tool.JsonSchema
            }));
        }

        return JsonSerializer.Serialize(
            catalog,
            new JsonSerializerOptions { WriteIndented = true });
    }

    public async Task<string> CallAsync(
        string server,
        string toolName,
        JsonElement arguments)
    {
        var client = Get(server);

        // Only invoke tools actually exposed by that server.
        var tools = await client.ListToolsAsync();

        if (!tools.Any(t => t.Name == toolName))
        {
            throw new ArgumentException(
                $"Tool '{toolName}' does not exist on '{server}'.");
        }

        var args = new Dictionary<string, object?>();

        if (arguments.ValueKind == JsonValueKind.Object)
        {
            foreach (var property in arguments.EnumerateObject())
            {
                args[property.Name] = property.Value.Clone();
            }
        }

        var result = await client.CallToolAsync(toolName, args);

        var output = string.Join(
            "\n",
            result.Content.Select(content =>
                content is TextContentBlock text
                    ? text.Text
                    : JsonSerializer.Serialize(content)));


       
        return result?.IsError == true
            ? $"Tool returned an error: {output}"
            : output;
    }
}


[McpServerToolType]
public  class GatewayTools
{
    [McpServerTool(Name = "list_backend_tools")]
    [Description("Discover tools exposed by the HR and IT MCP servers.")]
    public static async Task<string> ListBackendTools(
        BackendClients backends)
    {
        return await backends.DiscoverAsync();
    }

    [McpServerTool(Name = "call_backend_tool")]
    [Description(
        "Call a discovered backend tool. Provide server ('hr' or 'it'), " +
        "the exact toolName, and arguments as a JSON object.")]
    public static async Task<string> CallBackendTool(
        string server,
        string toolName,
        JsonElement arguments,
        BackendClients backends)
    {
        try
{
    return await backends.CallAsync(
        server,
        toolName,
        arguments);
}
catch (Exception ex)
{
    Console.Error.WriteLine(
        $"Backend tool failed: {server}/{toolName}");
    Console.Error.WriteLine(ex.ToString());

    throw;
}
    }
}
