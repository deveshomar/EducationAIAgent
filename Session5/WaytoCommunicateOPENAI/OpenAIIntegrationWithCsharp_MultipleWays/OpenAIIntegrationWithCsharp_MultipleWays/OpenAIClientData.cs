using OpenAI;
using OpenAI.Audio;
using OpenAI.Chat;
using OpenAI.Embeddings;
using OpenAI.Images;
using OpenAI.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAIIntegrationWithCsharp_MultipleWays
{
    public  class OpenAIClientData
    {

      public  static async Task CallAPI()
        {
            // Read API key
            var apiKey = "";

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                Console.WriteLine("OPENAI_API_KEY is not configured.");
                return;
            }

            // Create the parent OpenAI client
            OpenAIClient openAIClient =
                new OpenAIClient(apiKey);

            
            // Create ChatClient from OpenAIClient
            ChatClient chatClient =
                openAIClient.GetChatClient("gpt-5.1");


            // EmbeddingClient embeddingClient =
            //openAIClient.GetEmbeddingClient(
            //"text-embedding-3-small");

            //ResponsesClient responsesClient =
            //openAIClient.GetResponsesClient();

            //AudioClient audioClient =
            //openAIClient.GetAudioClient("tts-1");


            //ImageClient imageClient =
           // openAIClient.GetImageClient("gpt-image-1");


            // Send request
            ChatCompletion completion =
                await chatClient.CompleteChatAsync(
                    "Explain dependency injection in C#.");

            // Read response
            string answer =
                completion.Content[0].Text;

            Console.WriteLine("OpenAI Response:");
            Console.WriteLine("----------------");
            Console.WriteLine(answer);
        }

    }
}
