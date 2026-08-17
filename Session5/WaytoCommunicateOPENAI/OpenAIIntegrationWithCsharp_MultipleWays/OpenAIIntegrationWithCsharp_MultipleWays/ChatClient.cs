using OpenAI.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAIIntegrationWithCsharp_MultipleWays
{
    public class ChatClientOpenai
    {
        public static async Task CallAPI()
        {
            var apiKey = "";

            ChatClient client = new(
                model: "gpt-4o-mini",
                apiKey: apiKey);

            var messages = new List<ChatMessage>
{
                new SystemChatMessage("You are a .NET expert."),
                new UserChatMessage("Explain dependency injection."),
                
};

            ChatCompletion completion =
                await client.CompleteChatAsync(messages);

            string answer = completion.Content[0].Text;

        }
    }
}
