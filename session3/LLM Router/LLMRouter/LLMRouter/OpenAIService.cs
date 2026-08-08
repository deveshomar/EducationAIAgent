using OpenAI.Chat;

public class OpenAIService
{
    private readonly string _apiKey;

    public OpenAIService(IConfiguration configuration)
    {
        _apiKey = configuration["OpenAI:ApiKey"]!;
    }

    public async Task<string> AskAsync(string model, string prompt)
    {
        var client = new ChatClient(model, _apiKey);

        var response = await client.CompleteChatAsync(new[]
        {
            new UserChatMessage(prompt)
        });

        return response.Value.Content[0].Text;
    }
}