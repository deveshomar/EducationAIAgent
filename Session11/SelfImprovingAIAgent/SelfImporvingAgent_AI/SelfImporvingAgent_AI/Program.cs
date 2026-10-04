
using OpenAI.Chat;

// =====================================================
// SELF-IMPROVING INVESTMENT AGENT
// Simple C# teaching example
// =====================================================

// Get API key
string apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY")
                ?? "sk-proj-";

// OpenAI client
ChatClient client = new ChatClient(
    model: "gpt-4.1-mini",
    apiKey: apiKey
);

Console.WriteLine("======================================");
Console.WriteLine(" SELF-IMPROVING INVESTMENT AGENT");
Console.WriteLine("======================================");


// =====================================================
// USER REQUIREMENT
// =====================================================

string userRequest = """
I want to invest ₹20 lakh for 5 years.
My risk level is Moderate.
I need ₹5 lakh liquidity.
My objective is reasonable growth with controlled risk.
""";

Console.WriteLine("\nUSER REQUIREMENT:");
Console.WriteLine(userRequest);


// =====================================================
// STEP 1 - RESEARCH
// =====================================================

Console.WriteLine("\n======================================");
Console.WriteLine("STEP 1 - RESEARCH");
Console.WriteLine("======================================");

string researchPrompt = $"""
You are an investment research assistant.

User requirement:
{userRequest}

Do basic educational research.

Identify:

1. Suitable investment categories
2. Risk considerations
3. Liquidity considerations
4. Diversification considerations
5. Important assumptions

Do NOT create the final investment plan yet.

Keep the answer simple.
""";

ChatCompletion researchResponse =
    await client.CompleteChatAsync(researchPrompt);

string research =
    researchResponse.Content[0].Text;

Console.WriteLine("\nRESEARCH RESULT:");
Console.WriteLine(research);


// =====================================================
// STEP 2 - CREATE INITIAL PLAN
// =====================================================

Console.WriteLine("\n======================================");
Console.WriteLine("STEP 2 - CREATE INITIAL PLAN");
Console.WriteLine("======================================");

string planPrompt = $"""
You are an investment planning assistant.

User requirement:
{userRequest}

Research:
{research}

Create a simple educational investment plan.

Include:

- Suggested allocation
- Reason for allocation
- Risk considerations
- Liquidity considerations
- Diversification

Keep the plan simple.
""";

ChatCompletion planResponse =
    await client.CompleteChatAsync(planPrompt);

string plan =
    planResponse.Content[0].Text;

Console.WriteLine("\nINITIAL PLAN:");
Console.WriteLine(plan);


// =====================================================
// STEP 3 - EVALUATE INITIAL PLAN
// =====================================================

Console.WriteLine("\n======================================");
Console.WriteLine("STEP 3 - EVALUATE INITIAL PLAN");
Console.WriteLine("======================================");

string evaluationPrompt = $"""
You are a strict investment-plan evaluator.

User requirement:
{userRequest}

Investment plan:
{plan}

Evaluate the plan on:

1. Goal alignment
2. Risk alignment
3. Liquidity
4. Diversification
5. Overall quality

Give a score from 0 to 100.

Return ONLY these two lines:

score: 0
reason: short explanation
""";

ChatCompletion evaluationResponse =
    await client.CompleteChatAsync(evaluationPrompt);

string evaluation =
    evaluationResponse.Content[0].Text;

Console.WriteLine("\nEVALUATION:");
Console.WriteLine(evaluation);


// =====================================================
// STEP 4 - EXTRACT SCORE
// =====================================================

int score = ExtractScore(evaluation);

Console.WriteLine($"\nCURRENT SCORE: {score}");


// =====================================================
// STEP 5 - SELF IMPROVEMENT
// =====================================================

if (score < 90)
{
    Console.WriteLine("\n======================================");
    Console.WriteLine("STEP 4 - SELF IMPROVEMENT");
    Console.WriteLine("======================================");

    Console.WriteLine(
        $"Score {score} is below 90."
    );

    Console.WriteLine(
        "Agent will improve the investment plan..."
    );


    string improvePrompt = $"""
You are an investment planning agent.

Improve the current investment plan.

User requirement:
{userRequest}

Research:
{research}

Current investment plan:
{plan}

Evaluation:
{evaluation}

The current score is below 90.

Identify the weaknesses in the current plan.

Create an improved plan that specifically addresses
the evaluator's concerns.

Return only the improved investment plan.
""";

    ChatCompletion improvedResponse =
        await client.CompleteChatAsync(improvePrompt);

    plan =
        improvedResponse.Content[0].Text;

    Console.WriteLine("\nIMPROVED PLAN:");
    Console.WriteLine(plan);


    // =================================================
    // STEP 6 - EVALUATE IMPROVED PLAN
    // =================================================

    Console.WriteLine("\n======================================");
    Console.WriteLine("STEP 5 - EVALUATE IMPROVED PLAN");
    Console.WriteLine("======================================");

    string secondEvaluationPrompt = $"""
You are a strict investment-plan evaluator.

User requirement:
{userRequest}

Improved investment plan:
{plan}

Evaluate the improved plan on:

1. Goal alignment
2. Risk alignment
3. Liquidity
4. Diversification
5. Overall quality

Give a score from 0 to 100.

Return ONLY these two lines:

score: 0
reason: short explanation
""";

    ChatCompletion secondEvaluationResponse =
        await client.CompleteChatAsync(secondEvaluationPrompt);

    string secondEvaluation =
        secondEvaluationResponse.Content[0].Text;

    Console.WriteLine("\nSECOND EVALUATION:");
    Console.WriteLine(secondEvaluation);


    // Extract second score
    int secondScore =
        ExtractScore(secondEvaluation);

    Console.WriteLine(
        $"\nFINAL SCORE: {secondScore}"
    );
}


// =====================================================
// FINAL RESULT
// =====================================================

Console.WriteLine("\n======================================");
Console.WriteLine("FINAL INVESTMENT PLAN");
Console.WriteLine("======================================");

Console.WriteLine(plan);

Console.WriteLine("\n======================================");
Console.WriteLine("AGENT FINISHED");
Console.WriteLine("======================================");


// =====================================================
// HELPER METHOD
// =====================================================

static int ExtractScore(string evaluation)
{
    foreach (string line in evaluation.Split('\n'))
    {
        if (line.Trim()
            .StartsWith("score:", StringComparison.OrdinalIgnoreCase))
        {
            string value =
                line.Split(':', 2)[1].Trim();

            if (int.TryParse(value, out int score))
            {
                return score;
            }
        }
    }

    return 0;
}

