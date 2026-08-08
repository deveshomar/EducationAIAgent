using System.Text;
using System.Text.Json;
var apiKey = "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";

//var imagePath = "D:\\Sessions\\Proj\\Session2\\ImageAnalysis\\imageCAR.png";
var imagePath1 = "D:\\Sessions\\Proj\\Session2\\ImageAnalysis\\ImagePlay1.png";


var imageBytes = await File.ReadAllBytesAsync(imagePath1);
var base64Image = Convert.ToBase64String(imageBytes);

using var client = new HttpClient();

client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");


var requestBody = new
{
    model = "gpt-4.1-mini",

    input = new object[]
    {
        new
        {
            role = "user",
        

            content = new object[]
            {
                new
                {
                    type = "input_text",

                    text = @"
                        Analyze this image.
                        Extract all text those belong to car number format.

                        Return response in JSON format like:
                        {
  ""numbers"": []
}"
                },

                new
                {
                    type = "input_image",
                    image_url = $"data:image/png;base64,{base64Image}"
                }
            }
        }
    }
};


var requestBody_Game = new
{
    model = "gpt-4.1-mini",

    input = new object[]
    {
        new
        {
            role = "user",


            content = new object[]
            {
                new
                {
                    type = "input_text",

                    text = @"
                        Analyze this image. color of tshrit, color of pants, color of shoes,    
                        "
                },

                new
                {
                    type = "input_image",
                    image_url = $"data:image/png;base64,{base64Image}"
                }
            }
        }
    }
};


var json = JsonSerializer.Serialize(requestBody_Game);

var response = await client.PostAsync(
    "https://api.openai.com/v1/responses",
    new StringContent(json, Encoding.UTF8, "application/json")
);

var result = await response.Content.ReadAsStringAsync();

Console.WriteLine(result);