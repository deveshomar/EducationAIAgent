using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenAI.Chat;
using System.Text.Json;
namespace SimplePolicyRag.Eval
{


    public class RagMetricsEvaluator
    {
        private readonly ChatClient _chatClient;

        public RagMetricsEvaluator(ChatClient chatClient)
        {
            _chatClient = chatClient;
        }

        public async Task<RagMetrics> EvaluateAsync(
            string question,
            List<string> retrievedChunks,
            string answer)
        {
            string context = string.Join(
                "\n\n--- CHUNK ---\n\n",
                retrievedChunks);

            string prompt = "";

       

            ChatCompletion completion =
                await _chatClient.CompleteChatAsync(prompt);

            string json = completion.Content[0].Text;

            return JsonSerializer.Deserialize<RagMetrics>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })!;
        }
    }
}
