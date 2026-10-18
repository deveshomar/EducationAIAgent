
using System.Text.Json;
using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol;

//var endpoint = "http://localhost:5170/mcp"; 
var endpoint = "http://localhost:5000/mcp";   // this is for mcp gateway

var transport = new HttpClientTransport(
    new HttpClientTransportOptions
    {
        Endpoint = new Uri(endpoint)
    });

await using var client = await McpClient.CreateAsync(transport);

try
{
    // 1. Discover tools dynamically.
    var tools = await client.ListToolsAsync();

    Console.WriteLine("Available HR MCP tools:");

    for (var i = 0; i < tools.Count; i++)
    {
        Console.WriteLine(
            $"{i + 1}. {tools[i].Name} - {tools[i].Description}");
    }

    if (tools.Count == 0)
    {
        Console.WriteLine("No tools were discovered.");
        return;
    }

    // 2. Select a tool.
    Console.Write("\nSelect a tool number: ");

    if (!int.TryParse(Console.ReadLine(), out var selection) ||
        selection < 1 ||
        selection > tools.Count)
    {
        Console.WriteLine("Invalid selection.");
        return;
    }

    var selectedTool = tools[selection - 1];

    Console.WriteLine($"\nSelected: {selectedTool.Name}");
    Console.WriteLine("Input schema:");
    Console.WriteLine(selectedTool.JsonSchema);

    // 3. Read and validate JSON arguments.
    Console.WriteLine(
        "\nEnter arguments as JSON, matching the input schema.");

    Console.WriteLine(
        "Example: {\"employeeId\":\"EMP101\"}");

    Console.Write("Arguments: ");
    var json = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(json))
    {
        Console.WriteLine("Arguments cannot be empty.");
        return;
    }

    Console.WriteLine($"\nDEBUG Input received: [{json}]");

    Dictionary<string, object?> arguments;

    try
    {
        using var document = JsonDocument.Parse(json);

        if (document.RootElement.ValueKind != JsonValueKind.Object)
        {
            Console.WriteLine("Arguments must be a JSON object.");
            return;
        }

        arguments = new Dictionary<string, object?>();

        foreach (var property in document.RootElement.EnumerateObject())
        {
            arguments[property.Name] = property.Value.Clone();
        }
    }
    catch (JsonException ex)
    {
        Console.WriteLine($"Invalid JSON: {ex.Message}");
        return;
    }

    // 4. Invoke the selected MCP tool.
    Console.WriteLine($"\nInvoking {selectedTool.Name}...");

    var result = await client.CallToolAsync(
        selectedTool.Name,
        arguments);

    // 5. Display the result.
    Console.WriteLine("\nMCP tool result:");

    if (result.Content.Count == 0)
    {
        Console.WriteLine("(No content returned.)");
    }

    foreach (var content in result.Content)
    {
        if (content is TextContentBlock text)
        {
            Console.WriteLine(text.Text);
        }
        else
        {
            Console.WriteLine(JsonSerializer.Serialize(content));
        }
    }

    if (result?.IsError==true)
    {
        Console.WriteLine(
            "\nThe MCP tool reported an error. Check the server logs.");
    }
    else
    {
        Console.WriteLine("\nMCP tool invocation completed.");
    }
}
catch (Exception ex)
{
    Console.WriteLine("\nMCP operation failed:");
    Console.WriteLine(ex.ToString());
}
