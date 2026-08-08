using System;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

var apiKey = "s";
var url = "https://openrouter.ai/api/v1/chat/completions";

using var client = new HttpClient();

client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
client.DefaultRequestHeaders.Add("HTTP-Referer", "http://localhost");
client.DefaultRequestHeaders.Add("X-Title", "My C# Demo");

var request = new
{
    //model = "openai/gpt-4.1-mini",
    // model: "openrouter/auto",
    model = "google/gemini-2.5-pro",
    messages = new[]
    {
        new
        {
            role = "user",
            content = "Tell me five facts about India."
        }
    },

    temperature = 0.7,
    top_p = 1.0,
    max_tokens = 200
};

var json = JsonSerializer.Serialize(request);

var response = await client.PostAsync(
    url,
    new StringContent(json, Encoding.UTF8, "application/json"));

var responseString = await response.Content.ReadAsStringAsync();

Console.WriteLine(responseString);

//openai / gpt - 4.1 - mini
//openai / gpt - 5
//anthropic / claude - sonnet - 4
//google / gemini - 2.5 - pro
//meta - llama / llama - 3.3 - 70b - instruct
//deepseek / deepseek - chat