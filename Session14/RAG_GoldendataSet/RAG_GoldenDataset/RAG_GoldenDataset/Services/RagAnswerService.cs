using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAG_GoldenDataset.Services
{
    using OpenAI.Chat;

    public class RagAnswerService
    {
        private readonly ChatClient _chatClient;

        public RagAnswerService(string apiKey)
        {
            _chatClient = new ChatClient(
                model: "gpt-4.1-mini",
                apiKey: apiKey);
        }

        public async Task<string> GenerateAnswerAsync(
            string question,
            string context)
        {
            string prompt = $"""
        You are an employee policy assistant.

        Answer the user's question ONLY using the provided context.

        If the answer is not available in the context,
        say: "The information is not available in the provided policy."

        Context:
        {context}

        Question:
        {question}

        Answer:
        """;

            ChatCompletion response =
                await _chatClient.CompleteChatAsync(prompt);

            return response.Content[0].Text;
        }
    }
}
