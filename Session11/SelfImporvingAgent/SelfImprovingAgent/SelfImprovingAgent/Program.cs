using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using SelfImprovingAgent;
using SelfImprovingAgent.Agents;
using SelfImprovingAgent.Model;
using SelfImprovingAgent.OpenAIService;
using System.Text.Json;

// =====================================================
// 1. CREATE SEMANTIC KERNEL
// =====================================================

var builder = Kernel.CreateBuilder();
string key = "sgA";

builder.AddOpenAIChatCompletion(
    modelId: "gpt-4.1-mini",
    apiKey: key);

var kernel = builder.Build();


// =====================================================
// 2. GET CHAT COMPLETION SERVICE
// =====================================================

var chatService =
    kernel.GetRequiredService<IChatCompletionService>();


// =====================================================
// 3. CREATE LLM SERVICE
// =====================================================

var llm =
    new LlmService(chatService);


// =====================================================
// 4. CREATE AGENTS
// =====================================================

var requirementAgent =
    new RequirementAgent(llm);

var researchAgent =
    new ResearchAgent(llm);

var portfolioPlanner =
    new PortfolioPlanner(llm);

var evaluator =
    new PortfolioEvaluator(llm);


// =====================================================
// 5. CREATE SELF-IMPROVING AGENT
// =====================================================

var selfImprovingAgent =
    new SelfImprovingInvestmentAgent(
        researchAgent,
        portfolioPlanner,
        evaluator);


// =====================================================
// 6. CONVERSATIONAL REQUIREMENT COLLECTION
// =====================================================

InvestmentRequest? request = null;

while (true)
{
    Console.WriteLine();

    // ---------------------------------------------
    // First interaction
    // ---------------------------------------------

    if (request == null)
    {
        Console.Write(
            "Tell me your investment requirement: ");
    }
    else
    {
        // -----------------------------------------
        // Find missing information
        // -----------------------------------------

        var errors =
           Helpers. Validate(request);

        Console.WriteLine();
        Console.WriteLine(
            "I need some more information:");

        foreach (var error in errors)
        {
            Console.WriteLine(
                $"- {error}");
        }

        Console.WriteLine();
        Console.Write(
            "Your answer: ");
    }


    // ---------------------------------------------
    // Read user's message
    // ---------------------------------------------

    string userMessage =
        Console.ReadLine()!;


    // ---------------------------------------------
    // Extract / update requirements
    // ---------------------------------------------

    request =
        await requirementAgent.ExtractAsync(
            userMessage,
            request);


    // ---------------------------------------------
    // Validate updated request
    // ---------------------------------------------

    var validationErrors =
       Helpers. Validate(request);


    // ---------------------------------------------
    // If complete → leave conversation loop
    // ---------------------------------------------

    if (validationErrors.Count == 0)
    {
        break;
    }
}


// =====================================================
// 7. DISPLAY FINAL REQUIREMENTS
// =====================================================

Console.WriteLine();
Console.WriteLine(
    "======================================");

Console.WriteLine(
    "REQUIREMENTS COMPLETE");

Console.WriteLine(
    "======================================");

Console.WriteLine(
    JsonSerializer.Serialize(
        request,
        new JsonSerializerOptions
        {
            WriteIndented = true
        }));


// =====================================================
// 8. START SELF-IMPROVING AGENT
// =====================================================

Console.WriteLine();
Console.WriteLine(
    "======================================");

Console.WriteLine(
    "STARTING SELF-IMPROVING AGENT");

Console.WriteLine(
    "======================================");


// =====================================================
// 9. RUN AGENT LOOP
// =====================================================

var finalPortfolio =
    await selfImprovingAgent.RunAsync(
        request,
        targetScore: 90,
        maxIterations: 5);


// =====================================================
// 10. DISPLAY FINAL RESULT
// =====================================================

Console.WriteLine();
Console.WriteLine(
    "======================================");

Console.WriteLine(
    "FINAL PORTFOLIO");

Console.WriteLine(
    "======================================");

Console.WriteLine(
    JsonSerializer.Serialize(
        finalPortfolio,
        new JsonSerializerOptions
        {
            WriteIndented = true
        }));


// =====================================================
// VALIDATION METHODS
// =====================================================
