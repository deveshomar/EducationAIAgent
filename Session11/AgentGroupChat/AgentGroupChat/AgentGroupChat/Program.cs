
#pragma warning disable SKEXP0110



using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.Agents.Chat;
using Microsoft.SemanticKernel.ChatCompletion;

var builder = Kernel.CreateBuilder();
string apikey = "A";

builder.AddOpenAIChatCompletion(
    "gpt-4.1-mini",
   apikey);

var kernel = builder.Build();


// ==========================================
// 1. CREATE AGENTS
// ==========================================

var planner = new ChatCompletionAgent
{
    Name = "Planner",
    Instructions = """
    You are a Travel Planner.

    Create a simple 3-day Goa travel plan.

    The user has a budget of ₹30,000.

    If the Budget Checker says the plan is
    too expensive, improve the plan.

    Keep responses short.
    """,
    Kernel = kernel
};


var budgetChecker = new ChatCompletionAgent
{
    Name = "BudgetChecker",
    Instructions = """
    You are a Budget Checker.

    Check whether the travel plan is
    within ₹30,000.

    If it is too expensive, explain
    what should be changed.

    Keep responses short.
    """,
    Kernel = kernel
};


var reviewer = new ChatCompletionAgent
{
    Name = "Reviewer",
    Instructions = """
    You are the Final Reviewer.

    Review the travel plan and budget feedback.

    If the plan is good and within ₹30,000,
    reply with exactly:

    APPROVED

    Otherwise explain what needs improvement.

    Keep responses short.
    """,
    Kernel = kernel
};


// ==========================================
// 2. CREATE GROUP CHAT
// ==========================================

var chat = new AgentGroupChat(
    planner,
    budgetChecker,
    reviewer);


// ==========================================
// 3. TERMINATION STRATEGY
// ==========================================

var terminationFunction =
    AgentGroupChat.CreatePromptFunctionForStrategy(
        """
        Determine whether the Reviewer has
        approved the travel plan.

        If the Reviewer has approved,
        respond with YES.

        Otherwise respond with NO.

        Conversation:

        {{$history}}
        """,
        safeParameterNames: "history");


var terminationStrategy =
    new KernelFunctionTerminationStrategy(
        terminationFunction,
        kernel)
    {
        Agents = [reviewer],

        ResultParser = result =>
            result.GetValue<string>()?
                .Contains(
                    "YES",
                    StringComparison.OrdinalIgnoreCase)
            ?? false,

        HistoryVariableName = "history",

        MaximumIterations = 6
    };


chat.ExecutionSettings =
    new AgentGroupChatSettings
    {
        TerminationStrategy =
            terminationStrategy
    };


// ==========================================
// 4. USER REQUEST
// ==========================================

chat.AddChatMessage(
    new ChatMessageContent(
        AuthorRole.User,
        """
        Plan a 3-day Goa trip.

        My total budget is ₹30,000.

        Create a practical plan.
        """));


// ==========================================
// 5. RUN GROUP CHAT
// ==========================================

Console.WriteLine();
Console.WriteLine("===== GROUP CHAT =====");

await foreach (var response in chat.InvokeAsync())
{
    Console.WriteLine();
    Console.WriteLine(
        $"[{response.AuthorName}]");

    Console.WriteLine(
        response.Content);
}


Console.WriteLine();
Console.WriteLine("===== COMPLETED =====");