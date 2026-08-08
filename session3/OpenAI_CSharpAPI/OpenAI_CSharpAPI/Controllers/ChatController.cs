using Microsoft.AspNetCore.Mvc;
using OpenAI;
using OpenAI.Chat;
using OpenAI_CSharpAPI.Model;
using System.Text.Json;

[ApiController]
[Route("api/chat")]
public class ChatController : ControllerBase
{
    private readonly OpenAIClient _client;

    public ChatController(OpenAIClient client)
    {
        _client = client;
    }

    [HttpPost]
    public async Task<IActionResult> Chat(ChatRequest request)
    {
        var chatClient = _client.GetChatClient("gpt-5-mini");

        var result = await chatClient.CompleteChatAsync(
        [
            new UserChatMessage(request.Message)
        ]);

        string json = JsonSerializer.Serialize(
        result,
        new JsonSerializerOptions
        {
        WriteIndented = true
        });

        Console.WriteLine(json);


        return Ok(new ChatResponse
        {
            Answer = result.Value.Content[0].Text,
            RawJSON = json  

        });
    }
}