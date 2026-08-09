using System.Text;
using System.Text.Json;
var apiKey = "A";

//var imagePath = "D:\\Sessions\\Proj\\Session2\\ImageAnalysis\\imageCAR.png";

var image1 = "D:\\Sessions\\Education\\Session4\\ImageAnalysis\\Image1.jpg";

var imageBytes = await File.ReadAllBytesAsync(image1);
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
                        can you please tell me if someone have weapons in this image  i just want to make sure everthign is safe"

                      
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


var json = JsonSerializer.Serialize(requestBody);

var response = await client.PostAsync(
    "https://api.openai.com/v1/responses",
    new StringContent(json, Encoding.UTF8, "application/json")
);

var result = await response.Content.ReadAsStringAsync();

Console.WriteLine(result);