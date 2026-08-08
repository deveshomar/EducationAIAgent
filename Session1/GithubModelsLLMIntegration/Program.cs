using Newtonsoft.Json;
using System.Text;

Console.WriteLine("GitHub Models API Example");

string githubToken = "";

using HttpClient client = new HttpClient();

client.DefaultRequestHeaders.Add("Authorization", $"Bearer {githubToken}");

string url = "https://models.github.ai/inference/chat/completions";

var requestBody = new
{
    model = "openai/gpt-4.1-mini",
    messages = new[]
    {
        new
        {
            role = "user",
            content = "Explain RAG in simple words"
        }
    },
    temperature = 0.7,
    max_tokens = 200
};

var json = JsonConvert.SerializeObject(requestBody);

var httpContent = new StringContent(
    json,
    Encoding.UTF8,
    "application/json");

HttpResponseMessage response =
    await client.PostAsync(url, httpContent);

string responseText =
    await response.Content.ReadAsStringAsync();

Console.WriteLine(responseText);