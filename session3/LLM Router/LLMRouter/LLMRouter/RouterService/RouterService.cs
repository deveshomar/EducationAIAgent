using LLMRouter.Model;
using OpenAI.Chat;
using System.Text.Json;

public class RouterService
{
    private readonly string _apiKey;

    public RouterService(IConfiguration configuration)
    {
        _apiKey = configuration["OpenAI:ApiKey"]!;
    }

    public async Task<string> DecideAsync(string prompt)
    {
        var client = new ChatClient("gpt-5-nano", _apiKey);

            string systemPrompt =
            """
            You are an AI Router.

            Choose the best model.

            Available models

            gpt-5-nano
            - Classification
            - Rewrite
            - Grammar
            - Translation
            - Email

            gpt-5-mini
            - Chat
            - Summarization
            - RAG
            - Question Answering

            gpt-5
            - Coding
            - Research
            - Architecture
            - Debugging
            - Financial reasoning
            - Legal reasoning

            Return ONLY JSON.

            Example

            {
            "model":"gpt-5-mini",
            "reason":"Summarization",
            "complexity":"Medium",
            "confidence":0.95
            }
            """;

        ChatMessage[] messages =
        {
         new SystemChatMessage(systemPrompt),
         new UserChatMessage(prompt)
         };

        var response = await client.CompleteChatAsync(messages);

        string json = response.Value.Content[0].Text;
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        // ModelDecision decision = response.Value.GetOutput<ModelDecision>();

        //  return JsonSerializer.Deserialize<ModelDecision>(json)!;
        return json;
    }
}