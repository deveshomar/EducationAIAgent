using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;

public static class AgentFactory
{
    public static Kernel CreateEmployeeAgent(
        string apiKey)
    {
        var builder = Kernel.CreateBuilder();

        builder.AddOpenAIChatCompletion(
            modelId: "gpt-4.1-mini",
            apiKey: apiKey);

        builder.Plugins.AddFromType<EmployeePlugin>();

        return builder.Build();
    }

    public static Kernel CreateLeaveAgent(
    string apiKey)
    {
        var builder = Kernel.CreateBuilder();

        builder.AddOpenAIChatCompletion(
            modelId: "gpt-4.1-mini",
            apiKey: apiKey);

        builder.Plugins.AddFromType<LeavePlugin>();

        return builder.Build();
    }
    public static Kernel CreatePayrollAgent(
    string apiKey)
    {
        var builder = Kernel.CreateBuilder();

        builder.AddOpenAIChatCompletion(
            modelId: "gpt-4.1-mini",
            apiKey: apiKey);

        builder.Plugins.AddFromType<PayrollPlugin>();

        return builder.Build();
    }
    public static Kernel CreateITAgent(
    string apiKey)
    {
        var builder = Kernel.CreateBuilder();

        builder.AddOpenAIChatCompletion(
            modelId: "gpt-4.1-mini",
            apiKey: apiKey);

        builder.Plugins.AddFromType<ITPlugin>();

        return builder.Build();
    }
}