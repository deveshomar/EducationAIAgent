
using EmpAIAgent_MCP;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using ModelContextProtocol.Client;

var apiKey = "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";

if (string.IsNullOrWhiteSpace(apiKey))
{
    throw new InvalidOperationException(
        "Set the OPENAI_API_KEY environment variable.");
}

// 1. Create the Semantic Kernel builder.
var builder = Kernel.CreateBuilder();

builder.AddOpenAIChatCompletion(
    modelId: "gpt-4.1-mini",
    apiKey: apiKey);

var kernel = builder.Build();

// 2. Start and connect to your existing MCP server.
var transport = new StdioClientTransport(
    new StdioClientTransportOptions
    {
        Name = "Employee MCP Server",
        Command = "C:\\Program Files\\dotnet\\dotnet.exe",
        Arguments =
        [
            "run",
            "--project",
            @"D:\Sessions\Education\Session17\MCP\EmployeeMcpServer\EmployeeMcpServer\EmployeeMcpServer.csproj"
        ]
    });

await using var mcpClient = await McpClient.CreateAsync(transport);

// 3. Discover tools exposed by the MCP server.
var mcpTools = await mcpClient.ListToolsAsync();

Console.WriteLine("MCP tools discovered:");

foreach (var tool in mcpTools)
{
    Console.WriteLine($"- {tool.Name}: {tool.Description}");
}

// 4. Register the MCP adapter as a Semantic Kernel plugin.
kernel.Plugins.AddFromObject(
    new McpEmployeePlugin(mcpClient),
    "Employee");

// 5. Enable automatic AI function calling.
var settings = new OpenAIPromptExecutionSettings
{
    FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
};

// 6. Get the chat completion service and maintain history.
var chatService =
    kernel.GetRequiredService<IChatCompletionService>();

var history = new ChatHistory();

history.AddSystemMessage(
    """
    You are an employee assistance AI agent.
    Use the Employee plugin to retrieve employee information.
    Never invent employee details or leave balances.
    Base factual answers on the tool results.
    If the employee ID is missing or ambiguous, ask the user
    for the required information instead of guessing.
    """);

Console.WriteLine();
Console.WriteLine("Employee AI Agent is ready.");
Console.WriteLine("Ask a question or type 'exit' to stop.");
Console.WriteLine();

while (true)
{
    Console.Write("You: ");
    var input = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(input))
        continue;

    if (input.Equals("exit",
        StringComparison.OrdinalIgnoreCase))
        break;

    history.AddUserMessage(input);

    var response = await chatService.GetChatMessageContentAsync(
        history,
        executionSettings: settings,
        kernel: kernel);

    history.AddAssistantMessage(response.Content ?? "");

    Console.WriteLine($"Agent: {response}");
    Console.WriteLine();
}
