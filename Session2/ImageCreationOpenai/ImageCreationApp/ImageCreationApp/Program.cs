using OpenAI.Images;
var apiKey = "s";

//string apiKey = "YOUR_API_KEY";

var client = new ImageClient(
    model: "gpt-image-1",
    apiKey: apiKey);

Console.WriteLine("Enter Image Prompt:");
string prompt = Console.ReadLine()!;

var response = await client.GenerateImageAsync(
    prompt,
    new ImageGenerationOptions
    {
        Size = GeneratedImageSize.W1024xH1024
    });

string folder = Path.Combine(Directory.GetCurrentDirectory(), "Images");
Directory.CreateDirectory(folder);

string filePath = Path.Combine(
    folder,
    $"Image_{DateTime.Now:yyyyMMdd_HHmmss}.png");

await File.WriteAllBytesAsync(
    filePath,
    response.Value.ImageBytes.ToArray());

Console.WriteLine($"Image saved at:");
Console.WriteLine(filePath);