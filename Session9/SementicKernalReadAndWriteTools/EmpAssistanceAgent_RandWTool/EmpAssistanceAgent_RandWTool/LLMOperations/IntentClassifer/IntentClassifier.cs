using EmpAssistanceAgent_RandWTool.LLMOperations.IntentClassifer;
using Microsoft.SemanticKernel;
using System.Text.Json;

public class IntentClassifier
{
    private readonly Kernel _kernel;

    public IntentClassifier(Kernel kernel)
    {
        _kernel = kernel;
    }

    public async Task<IntentClassiferResponse> MakeLLMCallsAsync(
        string employeeId,
        string userMessage)
    {
        var prompt = """
You are an employee assistant router.
Also you can provide if the user request is a READ or WRITE operation.

Your job is to determine what operation the user wants.

Return ONLY valid JSON.

The action should identify the operation the user wants.

For READ operations, use the appropriate read action.

For WRITE operations, use the appropriate write action.

Do NOT execute any operation.
You are only classifying the user's request.

Return JSON in this format:

{
  "action": "",
  "arguments": {},
  "reason": ""
}

For leave application:
- action = "apply_leave"
- arguments must contain fromDate, toDate, reason

Example:

User: Apply leave tomorrow because of personal work.

Response:
{
  "action": "apply_leave",
  "R/W" :"True"
  "arguments": {
    "fromDate": "tomorrow",
    "toDate": "tomorrow",
    "reason": "personal work"
  },
  "reason": "User wants to apply for leave."
}

Now classify this user request:

User:
""";

        // Add the actual user message
        prompt += $"""
{userMessage}
""";

        var result =
            await _kernel.InvokePromptAsync(prompt);

        var response =
            result.ToString();

        Console.WriteLine(
            $"[LLM] {response}");

        var decision =
            JsonSerializer.Deserialize<IntentClassiferResponse>(
                response,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

        return decision ?? new IntentClassiferResponse();
    }
}

