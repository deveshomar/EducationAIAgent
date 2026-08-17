using System.Text;
using System.Text.Json;

var apiKey = "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";

if (string.IsNullOrWhiteSpace(apiKey))
{
    throw new Exception("OPENAI_API_KEY is not configured.");
}

var imagePaths = new[]
{
    @"D:\Sessions\Education\Session4\ImageAnalysisDetaied\Images\Image1.png",
    @"D:\Sessions\Education\Session4\ImageAnalysisDetaied\Images\Image2.png",
    @"D:\Sessions\Education\Session4\ImageAnalysisDetaied\Images\Image3.png"
};

var content = new List<object>();

// Prompt
content.Add(new
{
    type = "input_text",
    text = """
    Analyze ALL provided images independently and identify any clearly visible
    safety or dangerous situations.

    For each image, inspect specifically for:

    1. Vehicle accidents or collisions
       - Vehicle-to-vehicle collision
       - Vehicle-to-person collision
       - Crashed or overturned vehicle
       - Significant vehicle damage
       - Objects or vehicles blocking a roadway

    2. Weapons or potentially dangerous objects
       - A firearm visibly present
       - A knife or other visibly dangerous weapon
       - A person visibly holding a weapon
       - A person visibly carrying a potentially dangerous object

    3. Other visible safety hazards
       - Fire
       - Smoke
       - Explosion
       - Physical altercation
       - Person lying on the ground or apparently injured
       - Dangerous environmental conditions
       - Other clearly visible situations that could reasonably present an immediate safety risk

    IMPORTANT ANALYSIS RULES:

    - Analyze every image independently.
    - Do not assume that an object is a weapon if the image does not provide enough
      visual evidence. Mark it as "uncertain" when appropriate.
    - Do not infer a person's intentions, criminal behavior, or mental state.
    - Report only information that is visually supported by the image.
    - Do not classify a person as dangerous based on appearance, clothing,
      race, ethnicity, gender, age, or other personal characteristics.
    - If a weapon is clearly visible, report it.
    - If a person is clearly holding a weapon, report that the person is visibly
      holding the object. Do not infer their intent.
    - If an accident is visible, describe the observable evidence such as
      collision, damaged vehicles, overturned vehicles, debris, etc.
    - Do not report a danger simply because something looks unusual.
    - When the evidence is ambiguous, set "certainty" to "uncertain".
    - Confidence should represent how clearly the visual evidence supports the detection.
    - Return JSON only. Do not include markdown or explanatory text outside the JSON.

    Use this exact JSON structure:

    {
      "overallRisk": "LOW|MEDIUM|HIGH",
      "dangerDetected": true,
      "summary": "Short summary of the most important visible safety findings.",
      "images": [
        {
          "imageId": "image1",
          "dangerDetected": true,
          "riskLevel": "LOW|MEDIUM|HIGH",
          "detections": [
            {
              "type": "vehicle_accident|weapon_visible|person_holding_weapon|fire|smoke|physical_altercation|possible_injury|dangerous_object|other",
              "description": "Describe exactly what is visibly present.",
              "confidence": 0.0,
              "certainty": "certain|uncertain",
              "evidence": "Brief description of the visual evidence supporting the detection."
            }
          ]
        }
      ]
    }
    """
});

// Add images
for (int i = 0; i < imagePaths.Length; i++)
{
    var imagePath = imagePaths[i];

    if (!File.Exists(imagePath))
        continue;

    var bytes = await File.ReadAllBytesAsync(imagePath);

    var base64 = Convert.ToBase64String(bytes);

    var extension = Path.GetExtension(imagePath)
        .ToLowerInvariant();

    var mimeType = extension switch
    {
        ".jpg" or ".jpeg" => "image/jpeg",
        ".png" => "image/png",
        ".webp" => "image/webp",
        ".gif" => "image/gif",
        _ => "image/png"
    };

    content.Add(new
    {
        type = "input_text",
        text = $"The following image is image{i + 1}."
    });

    content.Add(new
    {
        type = "input_image",
        image_url = $"data:{mimeType};base64,{base64}"
    });
}

var requestBody = new
{
    model = "gpt-4.1-mini",

    input = new object[]
    {
        new
        {
            role = "user",
            content = content
        }
    }
};

var json = JsonSerializer.Serialize(requestBody);

using var client = new HttpClient();

client.DefaultRequestHeaders.Add(
    "Authorization",
    $"Bearer {apiKey}"
);

var response = await client.PostAsync(
    "https://api.openai.com/v1/responses",
    new StringContent(
        json,
        Encoding.UTF8,
        "application/json"));

Console.WriteLine("Processing Images");

var result = await response.Content.ReadAsStringAsync();

Console.WriteLine(result);