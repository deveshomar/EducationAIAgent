using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;

public class SupervisorAgent
{
    private readonly Kernel _kernel;

    public SupervisorAgent(string apiKey)
    {
        var builder = Kernel.CreateBuilder();

        builder.AddOpenAIChatCompletion(
            modelId: "gpt-4.1-mini",
            apiKey: apiKey);

        _kernel = builder.Build();
    }

    public async Task<string> RouteAsync(string userQuery)
    {
        var prompt = $"""
        You are a Supervisor Agent.

        Your job is to identify which specialist agent
        should handle the user's request.

        Available agents:

        EMPLOYEE
        - Employee details
        - Manager details
        - Team members

        LEAVE
        - Leave balance
        - Leave history
        - Apply leave
        - Cancel leave

        PAYROLL
        - Salary
        - Salary slip

        IT
        - IT helpdesk
        - IT ticket

        Return ONLY one of:

        EMPLOYEE
        LEAVE
        PAYROLL
        IT

        User request:
        {userQuery}
        """;

        var result = await _kernel.InvokePromptAsync(prompt);

        return result.ToString().Trim();
    }
}