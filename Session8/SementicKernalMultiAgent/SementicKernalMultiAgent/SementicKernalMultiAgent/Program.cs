using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;

string apiKey = "";


var supervisor = new SupervisorAgent(apiKey);

var employeeAgent =
    AgentFactory.CreateEmployeeAgent(apiKey);

var leaveAgent =
    AgentFactory.CreateLeaveAgent(apiKey);

var payrollAgent =
    AgentFactory.CreatePayrollAgent(apiKey);

var itAgent =
    AgentFactory.CreateITAgent(apiKey);

Console.WriteLine("====================================");
Console.WriteLine("       MULTI AGENT EMPLOYEE APP");
Console.WriteLine("====================================");

while (true)
{
    Console.Write("\nUser: ");

    var userQuery = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(userQuery))
        continue;

    if (userQuery.Equals("exit",
        StringComparison.OrdinalIgnoreCase))
        break;

    // 1. Supervisor decides
    var route =
        await supervisor.RouteAsync(userQuery);

    Console.WriteLine(
        $"Supervisor → {route}");

    // 2. Execute selected agent

    var settings = new OpenAIPromptExecutionSettings
    {
        FunctionChoiceBehavior =
            FunctionChoiceBehavior.Auto()
    };

    string response;

    switch (route)
    {
        case "EMPLOYEE":

            response = (await employeeAgent.InvokePromptAsync(
                userQuery,
                new(settings))).ToString();

            break;

        case "LEAVE":

            response = (await leaveAgent.InvokePromptAsync(
                userQuery,
                new(settings))).ToString();

            break;

        case "PAYROLL":

            response = (await payrollAgent.InvokePromptAsync(
                userQuery,
                new(settings))).ToString();

            break;

        case "IT":

            response = (await itAgent.InvokePromptAsync(
                userQuery,
                new(settings))).ToString();

            break;

        default:

            response =
                "Sorry, I could not determine the appropriate agent.";

            break;
    }

    Console.WriteLine($"\nAssistant: {response}");
}