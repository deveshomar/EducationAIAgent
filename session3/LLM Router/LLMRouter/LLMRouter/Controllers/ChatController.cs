using LLMRouter.Request;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/chat")]
public class ChatController : ControllerBase
{
    private readonly RouterService _router;
    private readonly OpenAIService _openAI;

    public ChatController(
        RouterService router,
        OpenAIService openAI)
    {
        _router = router;
        _openAI = openAI;
    }

    [HttpPost]
    public async Task<IActionResult> Chat(ChatRequest request)
    {
        var decision = await _router.DecideAsync(request.Prompt);

        // nano  Write a professional leave email.
        // expalain microservice i want to host at vm and need to expose api to public share security

        //string model = "";
        //var answer = await _openAI.AskAsync(
        // decision.Model,
        // request.Prompt);

        return Ok(new
        {
            decision
        });
    }
}