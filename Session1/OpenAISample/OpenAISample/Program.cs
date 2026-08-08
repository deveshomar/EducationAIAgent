using System.Text;
using System.Text.Json;

var apiKey = "";
var url = "https://api.openai.com/v1/responses";

using var client = new HttpClient();
client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

var requestBody = new
{
    model = "gpt-4.1-mini",
    input = "can you tell me 5 lines for india"
};

var json = JsonSerializer.Serialize(requestBody);
var content = new StringContent(json, Encoding.UTF8, "application/json");

var response = await client.PostAsync(url, content);
var responseString = await response.Content.ReadAsStringAsync();

Console.WriteLine(responseString);



//using JsonDocument doc = JsonDocument.Parse(responseString);
//var output = doc.RootElement
//    .GetProperty("output")[0]
//    .GetProperty("content")[0]
//    .GetProperty("text")
//    .GetString();

//Console.WriteLine(output);
