using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OpenAIIntegrationWithCsharp_MultipleWays
{
    public class DirectHTTPCall
    {
        public async Task CallOpenAI()
        {
            var apiKey = "";
            using var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", apiKey);

            var request = new
            {
                model = "gpt-5.6",
                input = "Explain dependency injection in C#"
            };

            var json = JsonSerializer.Serialize(request);

            var response = await httpClient.PostAsync(
                "https://api.openai.com/v1/responses",
                new StringContent(json, Encoding.UTF8, "application/json"));

            var result = await response.Content.ReadAsStringAsync();

            Console.WriteLine(result);

        }
    }
}
