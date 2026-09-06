
using MultiAgentOrchUsingSementicKernal.Agents;
using MultiAgentOrchUsingSementicKernal.Router;

Console.WriteLine("=================================");
Console.WriteLine(" Employee AI Assistant");
Console.WriteLine(" Type 'exit' to quit");
Console.WriteLine("=================================");

var supervisor = new SupervisorAgent();
var router = new AgentExecuter();

while (true)
{
    Console.WriteLine();
    Console.Write("Ask Question : ");

    var question = Console.ReadLine();

    if (string.Equals(question, "exit", StringComparison.OrdinalIgnoreCase))
        break;

    if (string.IsNullOrWhiteSpace(question))
        continue;

    Console.WriteLine();

    // Step 1 : Decide which agents are required
    var agents = await supervisor.DecideAsync(question);

    Console.WriteLine("Selected Agents:");

    foreach (var agent in agents)
    {
        Console.WriteLine($" - {agent}");
    }

    Console.WriteLine();

    // Step 2 : Execute selected agents
    var responses = await router.ExecuteAsync(agents, question);

    Console.WriteLine("Responses");
    Console.WriteLine("--------------------------------");

    foreach (var response in responses)
    {
        Console.WriteLine(response);
        Console.WriteLine("--------------------------------");
    }
}

Console.WriteLine("Good Bye...");
