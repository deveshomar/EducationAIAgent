using Microsoft.SemanticKernel;
using MulltiAgentSementicKernal.AGENTS;
using MulltiAgentSementicKernal.Router;

var builder = Kernel.CreateBuilder();

var apiKeyLpenaI = "";



builder.AddOpenAIChatCompletion(
    modelId: "gpt-4.1-mini",
    apiKey: apiKeyLpenaI);

var kernel = builder.Build();

var supervisor = new SupervisorAgent(kernel);
var router = new AgentRouter(kernel);

Console.WriteLine("=========================================");
Console.WriteLine(" Employee Management AI Assistant");
Console.WriteLine(" Type 'exit' to quit");
Console.WriteLine("=========================================");

while (true)
{
    Console.WriteLine();
    Console.Write("Ask your question: ");

    var question = Console.ReadLine();

    // Exit condition
    if (string.Equals(question, "exit", StringComparison.OrdinalIgnoreCase))
    {
        Console.WriteLine("Goodbye!");
        break;
    }

    // Ignore empty input
    if (string.IsNullOrWhiteSpace(question))
    {
        Console.WriteLine("Please enter a valid question.");
        continue;
    }

    try
    {
        // Step 1: Supervisor decides which agents are needed
        var selectedAgents = await supervisor.DecideAgents(question);

        Console.WriteLine();
        Console.WriteLine("Selected Agents:");
        Console.WriteLine("----------------");

        foreach (var agent in selectedAgents)
        {
            Console.WriteLine($"- {agent}");
        }

        // Step 2: Execute selected agents in parallel
        var responses = await router.ExecuteAsync(selectedAgents, question);

        Console.WriteLine();
        Console.WriteLine("Responses:");
        Console.WriteLine("----------");

        foreach (var response in responses)
        {
            Console.WriteLine(response);
            Console.WriteLine("--------------------------------------");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}