#pragma warning disable OPENAI001

using OpenAI.Responses;

// =====================================================
// 1. Get API Key
// =====================================================

var apiKey = "";


if (string.IsNullOrWhiteSpace(apiKey))
{
    throw new Exception(
        "OPENAI_API_KEY is not configured."
    );
}

// =====================================================
// 2. Create OpenAI Responses Client
// =====================================================

ResponsesClient client = new ResponsesClient(
    apiKey: apiKey
);

// =====================================================
// 3. Create Web Search Options
// =====================================================

var requestBody = new CreateResponseOptions
{
    Model = "gpt-5",

    //Tools =
    //{
    //    ResponseTool.CreateWebSearchTool()
    //}
};

// =====================================================
// 4. Start Interactive Loop
// =====================================================

Console.WriteLine("==========================================");
Console.WriteLine("      OpenAI Web Search Demo");
Console.WriteLine("==========================================");
Console.WriteLine();
Console.WriteLine("Ask any question.");
Console.WriteLine("Type 'exit' to close the application.");
Console.WriteLine();

while (true)
{
    Console.Write("You: ");

    var userInput = Console.ReadLine();

    // Handle empty input
    if (string.IsNullOrWhiteSpace(userInput))
    {
        continue;
    }

    // Exit application
    if (userInput.Equals(
        "exit",
        StringComparison.OrdinalIgnoreCase))
    {
        Console.WriteLine();
        Console.WriteLine("Goodbye!");
        break;
    }

    try
    {
        // =================================================
        // 5. Create User Message
        // =================================================

        requestBody.InputItems.Clear();

        requestBody.InputItems.Add(
            ResponseItem.CreateUserMessageItem(
                userInput
            )
        );

        Console.WriteLine();
        Console.WriteLine("Calling Open AI......");
        Console.WriteLine();

        // =================================================
        // 6. Call OpenAI
        // =================================================

        ResponseResult response =
            await client.CreateResponseAsync(requestBody);

        // =================================================
        // 7. Display Answer
        // =================================================




        Console.WriteLine("========== ANSWER ==========");
        Console.WriteLine();




        Console.WriteLine(
            response.GetOutputText()
        );

        Console.WriteLine();
        Console.WriteLine("============================");
        Console.WriteLine();


        Console.WriteLine("========== OUTPUT ITEMS ==========");

        bool webSearchUsed = false;

        foreach (var item in response.OutputItems)
        {
            if (item.GetType().Name.Contains("WebSearch"))
            {
                webSearchUsed = true;
            }
        }

        Console.WriteLine(
            $"Web Search Used: {webSearchUsed}"
        );
    }
    catch (Exception ex)
    {
        Console.WriteLine();
        Console.WriteLine("ERROR:");
        Console.WriteLine(ex.Message);
        Console.WriteLine();
    }
}