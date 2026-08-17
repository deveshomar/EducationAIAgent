using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenAI.Responses;
namespace OpenAIIntegrationWithCsharp_MultipleWays
{
    public class OpenAIReponseClientOpenAI
    {
        public static async Task CallOpenAI()
        {

#pragma warning disable OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            var apiKey = "";
            ResponsesClient client = new(
                apiKey: apiKey);
                
            
            #pragma warning restore OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            #pragma warning disable OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
          
                ResponseResult response =
                await client.CreateResponseAsync(
                    "gpt-5.1",
                    "Explain dependency injection.");



                    #pragma warning restore OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

                    string result = response.GetOutputText();
                    Console.WriteLine(result);

        }
    }
}
