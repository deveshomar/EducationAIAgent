using System.Text;
using System.Text.Json;
var apiKey = "";

//var apiKey = "YOUR_API_KEY";
var url = "https://api.openai.com/v1/responses";

using var client = new HttpClient();
client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

double temperature = .2;      // Change this value  .1 to 2   , higer the value high will be creaticty
double topP = 1.0;            // Keep 1.0 if using temperature
int maxTokens = 500;

var requestBody = new
{
    model = "gpt-4.1-mini",
    input = "I have to submit report for my manager for Mobile prodduct can you do some research and create",
    temperature = temperature,
    top_p = topP,
    max_output_tokens = maxTokens,
   
};

var json = JsonSerializer.Serialize(requestBody);

var content = new StringContent(
    json,
    Encoding.UTF8,
    "application/json");

var response = await client.PostAsync(url, content);

response.EnsureSuccessStatusCode();

var responseString = await response.Content.ReadAsStringAsync();

using JsonDocument doc = JsonDocument.Parse(responseString);

string? output = doc.RootElement
    .GetProperty("output")[0]
    .GetProperty("content")[0]
    .GetProperty("text")
    .GetString();

Console.WriteLine(output);