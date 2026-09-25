using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace SelfImprovingAgent.OpenAIService
{


    public class LlmService
    {
        private readonly IChatCompletionService _chatService;

        public LlmService(IChatCompletionService chatService)
        {
            _chatService = chatService;
        }

        public async Task<string> AskAsync(string prompt)
        {
            var history = new ChatHistory();

            history.AddUserMessage(prompt);

            var response =
                await _chatService.GetChatMessageContentAsync(history);

            return response.Content ?? "";
        }
    }
}
