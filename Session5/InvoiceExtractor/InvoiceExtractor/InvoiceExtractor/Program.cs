using InvoiceExtractor.Model;
using OpenAI.Chat;
using System.Text.Json;

string imagePath = @"D:\Sessions\Education\Session5\InvoiceExtractor\InvalidData.png";
byte[] imageBytes = await File.ReadAllBytesAsync(imagePath);

Console.WriteLine($"Read {imageBytes.Length} bytes from {imagePath}");

var apiKey = "";

// Create a ChatClient instance with the specified model and API key    

ChatClient client = new ChatClient(
    model: "gpt-4.1-mini",
    apiKey: apiKey);

// Create a prompt for extracting invoice data from the image   
string extractionPrompt = """
Extract the invoice data from the image.

Return ONLY valid JSON.

CRITICAL JSON RULES:

1. Do not wrap JSON in markdown.
2. Do not use ```json.
3. Quantity MUST be a JSON number.
4. UnitCost MUST be a JSON number.
5. Amount MUST be a JSON number.
6. SubTotal MUST be a JSON number.
7. GstPercentage MUST be a JSON number.
8. GstAmount MUST be a JSON number.
9. TotalAmount MUST be a JSON number.
10. Never include currency symbols.
11. Never include commas inside numeric values.
12. You do not need to calculate any values. Just extract the values from the image.
13. I am expecting JSON Fields have valid Values from Image else Put blank


Example:

{
  "InvoiceNumber": "INV-2025-00078",
  "BillingDate": "15 May 2025",
  "Items": [
    {
      "ProductName": "Wireless Mouse",
      "SerialNumber": "WM2025A1001",
      "Quantity": 2,
      "UnitCost": 650.00,
      "Amount": 1300.00
    }
  ],
  "SubTotal": 6950.00,
  "GstPercentage": 18.00,
  "GstAmount": 1251.00,
  "TotalAmount": 99988.00
}

Return ONLY the JSON object.
""";

// Create a list of chat messages with the extraction prompt and the image  
var messages = new List<ChatMessage>
{
    new UserChatMessage(
        ChatMessageContentPart.CreateTextPart(extractionPrompt),

        ChatMessageContentPart.CreateImagePart(
            BinaryData.FromBytes(imageBytes),
            "image/png",
            ChatImageDetailLevel.High))
};

Console.WriteLine("Sending request to OpenAI...");

// Send the chat messages to the OpenAI API and get the response    
ChatCompletion completion =
    await client.CompleteChatAsync(messages);

// Extract the JSON string from the response and clean it up    
string json = completion.Content[0].Text;

Console.WriteLine(json);

json = json
    .Replace("```json", "")
    .Replace("```", "")
    .Trim();

// Deserialize the JSON string into an Invoice object, ignoring case sensitivity for property names
Invoice invoice = JsonSerializer.Deserialize<Invoice>(
    json,
    new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    })!;

Console.WriteLine($"Invoice Number: {invoice.InvoiceNumber}");
Console.WriteLine($"Billing Date: {invoice.BillingDate}");
Console.WriteLine("Validating Result");

InvoiceValidationResult validation =
    ValidationRule.ValidateInvoice(invoice);

if (validation.IsValid)
{
    Console.WriteLine("Invoice is VALID");
}
else
{
    Console.WriteLine("Invoice is INVALID");
    Console.WriteLine("=====================");
    Console.WriteLine("=========Reasons are=========");
    foreach (var error in validation.Errors)
    {
        Console.WriteLine(error);
    }
}