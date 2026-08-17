using OpenAI.Chat;
var apiKey = "";


var client = new ChatClient(
    model: "gpt-5.5",
    apiKey: apiKey
);

// MEMORY
List<ChatMessage> messages = new List<ChatMessage>();

// SYSTEM MESSAGE
messages.Add(
    ChatMessage.CreateSystemMessage(
        "You are a helpful AI assistant."
    )
);

Console.WriteLine("Type 'exit' to close.");

while (true)
{
    Console.Write("\nUser: ");

    string userInput = Console.ReadLine();

    if (userInput?.ToLower() == "exit")
        break;

    // ADD USER MESSAGE TO MEMORY
    messages.Add(
        ChatMessage.CreateUserMessage(userInput)
    );

    // SEND COMPLETE HISTORY
    ChatCompletion completion =
        await client.CompleteChatAsync(messages);

    string response =
        completion.Content[0].Text;

    Console.WriteLine($"\nAI: {response}");

    // SAVE AI RESPONSE INTO MEMORY
    messages.Add(
        ChatMessage.CreateAssistantMessage(response)
    );
}