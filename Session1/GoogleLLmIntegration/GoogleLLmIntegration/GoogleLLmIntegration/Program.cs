using Newtonsoft.Json;
using System.Text;

Console.WriteLine("This is Google Generative Language API integration example.");   

string apiKey = "";


using HttpClient client = new HttpClient();



string url =
    $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={apiKey}";

var requestBody = new
{
    contents = new[]
    {
        new
        {
            parts = new[]
            {
                new
                {
                    text = "can you tell me about india in 100 words"
                }
            }
        }
    }
};

var json = JsonConvert.SerializeObject(requestBody);

Console.WriteLine(json);

var httpContent = new StringContent(
    json,
    Encoding.UTF8,
    "application/json");

HttpResponseMessage response =
    await client.PostAsync(url, httpContent);

string responseText =
    await response.Content.ReadAsStringAsync();

Console.WriteLine(responseText);
