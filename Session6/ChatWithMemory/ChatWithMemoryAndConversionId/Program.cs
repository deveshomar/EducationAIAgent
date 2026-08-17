//using ChatWithMemoryAndConversionId.FILEIO;
//using ChatWithMemoryAndConversionId.Model;
//using OpenAI.Chat;
//using System.Text.Json;



//var client = new ChatClient(
//    model: "gpt-5.5",
//    apiKey: apiKey
//);


//Console.Write("Enter Conversation Id: ");

//string conversationId = Console.ReadLine();

//if (string.IsNullOrWhiteSpace(conversationId))
//{
//    conversationId = Guid.NewGuid().ToString();

//    Console.WriteLine($"New Conversation Created: {conversationId}");
//}
//string fileName = @"D:\Sessions\Education\Session5\ChatWithMemory\ChatWithMemoryAndConversionId\json\" + conversationId +".json";

//// ALL MESSAGES STORED HERE
//List<ConversationMessage> savedMessages = new();

//// LOAD OLD CONVERSATION IF EXISTS
//if (File.Exists(fileName))
//{
//    string oldJson = await File.ReadAllTextAsync(fileName);
//    savedMessages =
//        JsonSerializer.Deserialize<List<ConversationMessage>>(oldJson)
//        ?? new List<ConversationMessage>();

//    Console.WriteLine("Old conversation loaded.");
//}
//else
//{
//    Console.WriteLine("New conversation started.");

//    // ADD SYSTEM MESSAGE FIRST TIME ONLY
//    savedMessages.Add(new ConversationMessage
//    {
//        ConversationId = conversationId,
//        Role = "system",
//        Content = "You are a helpful AI assistant.",
//        CreatedDate = DateTime.Now
//    });

//    await FileOperations.SaveMessages(fileName, savedMessages);
//}

//// CONVERT SAVED MESSAGES TO OPENAI FORMAT
//List<ChatMessage> messages = new();

//foreach (var item in savedMessages)
//{
//    if (item.Role == "system")
//    {
//        messages.Add(
//            ChatMessage.CreateSystemMessage(item.Content)
//        );
//    }
//    else if (item.Role == "user")
//    {
//        messages.Add(
//            ChatMessage.CreateUserMessage(item.Content)
//        );
//    }
//    else if (item.Role == "assistant")
//    {
//        messages.Add(
//            ChatMessage.CreateAssistantMessage(item.Content)
//        );
//    }
//}

//Console.WriteLine("Type 'exit' to close.");

//while (true)
//{
//    Console.Write("\nUser: ");

//    string userInput = Console.ReadLine();

//    if (userInput?.ToLower() == "exit")
//        break;

//    // ADD USER MESSAGE TO OPENAI MEMORY
//    messages.Add(
//        ChatMessage.CreateUserMessage(userInput)
//    );

//    // SAVE USER MESSAGE
//    savedMessages.Add(new ConversationMessage
//    {
//        ConversationId = conversationId,
//        Role = "user",
//        Content = userInput,
//        CreatedDate = DateTime.Now
//    });

//    // SEND COMPLETE HISTORY TO OPENAI
//    ChatCompletion completion =
//        await client.CompleteChatAsync(messages);

//    string aiResponse =
//        completion.Content[0].Text;

//    Console.WriteLine($"\nAI: {aiResponse}");

//    // ADD AI RESPONSE TO MEMORY
//    messages.Add(
//        ChatMessage.CreateAssistantMessage(aiResponse)
//    );

//    // SAVE AI RESPONSE
//    savedMessages.Add(new ConversationMessage
//    {
//        ConversationId = conversationId,
//        Role = "assistant",
//        Content = aiResponse,
//        CreatedDate = DateTime.Now
//    });

//    // SAVE FILE
//    await FileOperations.SaveMessages(fileName, savedMessages);
//}


//// MODEL



using ChatWithMemoryAndConversionId;
using ChatWithMemoryAndConversionId.FILEIO;
using ChatWithMemoryAndConversionId.Model;
using OpenAI.Chat;
using System.Text.Json;

const string model = "gpt-5.5";

var apiKey = "";


var client = new ChatClient(
    model: model,
    apiKey: apiKey);

// ----------------------------------------------------
// 1. Get Conversation ID
// ----------------------------------------------------

string conversationId = Helper.GetConversationId(); // go db and check

Console.WriteLine($"Conversation Id: {conversationId}");


// ----------------------------------------------------
// 2. Get conversation file
// ----------------------------------------------------

string fileName = GetConversationFileName(conversationId);

Console.WriteLine($"Conversation File: {fileName}");


// ----------------------------------------------------
// 3. Load conversation
// ----------------------------------------------------

List<ConversationMessage> savedMessages =
    await FileOperations.LoadConversationAsync(fileName);


// ----------------------------------------------------
// 4. Initialize new conversation if required
// ----------------------------------------------------

if (savedMessages.Count == 0)
{
    await InitializeConversationAsync(
        conversationId,
        savedMessages,
        fileName);
}


// ----------------------------------------------------
// 5. Convert saved messages to OpenAI messages
// ----------------------------------------------------

// we need to pass to opnenai
List<ChatMessage> messages =
    ConvertToOpenAIMessages(savedMessages);


// ----------------------------------------------------
// 6. Start chat loop
// ----------------------------------------------------

Console.WriteLine("Type 'exit' to close.");

while (true)
{
    Console.Write("\nUser: ");

    string? userInput = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(userInput))
        continue;

    if (userInput.Equals(
        "exit",
        StringComparison.OrdinalIgnoreCase))
    {
        break;
    }

    // --------------------------------------------
    // Add user message
    // --------------------------------------------

    AddUserMessage(
        conversationId,
        userInput,
        messages,
        savedMessages);


    // --------------------------------------------
    // Call OpenAI
    // --------------------------------------------

    //List<ChatMessage>  newMessage=messages.Where(m => m.Role == "user" || m.Role == "assistant").ToList();

    if(messages.Count > 100)
    {
        messages.Add(
              ChatMessage.CreateUserMessage(
                  "You need to summarixe this to me and i want ot send to LLM"));

        string aisummary =
           await GetAIResponseAsync(
               client,
               messages);
    }
    // MAKE Another LLM CALL
    


    string aiResponse =
        await GetAIResponseAsync(
            client,
            messages);


    // --------------------------------------------
    // Display response
    // --------------------------------------------

    Console.WriteLine($"\nAI: {aiResponse}");


    // --------------------------------------------
    // Add assistant response
    // --------------------------------------------

    AddAssistantMessage(
        conversationId,
        aiResponse,
        messages,
        savedMessages);


    // --------------------------------------------
    // Save conversation
    // --------------------------------------------

    await FileOperations. SaveConversationAsync(
        fileName,
        savedMessages);
}




static string GetConversationFileName(
    string conversationId)
{
    return Path.Combine(
        @"D:\Sessions\Education\Session6\ChatWithMemory\ChatWithMemoryAndConversionId\json",
        $"{conversationId}.json");
}





static async Task InitializeConversationAsync(
    string conversationId,
    List<ConversationMessage> savedMessages,
    string fileName)
{
    Console.WriteLine(
        "New conversation started.");

    var systemMessage = new ConversationMessage
    {
        ConversationId = conversationId,
        Role = "system",
        Content = "You are a helpful AI assistant.",
        CreatedDate = DateTime.Now
    };

    savedMessages.Add(systemMessage);

    await FileOperations. SaveConversationAsync(
        fileName,
        savedMessages);
}


// ------------------------------------------------------------


static List<ChatMessage> ConvertToOpenAIMessages(
    List<ConversationMessage> savedMessages)
{
    var messages = new List<ChatMessage>();

    foreach (var item in savedMessages)
    {
        switch (item.Role.ToLowerInvariant())
        {
            case "system":

                messages.Add(
                    ChatMessage.CreateSystemMessage(
                        item.Content));
                break;
            case "user":
                messages.Add(
                    ChatMessage.CreateUserMessage(
                        item.Content));
                break;
            case "assistant":

                messages.Add(
                    ChatMessage.CreateAssistantMessage(
                        item.Content));

                break;
        }
    }

    return messages;
}


// ------------------------------------------------------------


static void AddUserMessage(
    string conversationId,
    string userInput,
    List<ChatMessage> messages,
    List<ConversationMessage> savedMessages)
{
    // Add to OpenAI conversation
    messages.Add(
        ChatMessage.CreateUserMessage(
            userInput));


    // Add to persistent conversation
    savedMessages.Add(
        new ConversationMessage
        {
            ConversationId = conversationId,
            Role = "user",
            Content = userInput,
            CreatedDate = DateTime.Now
        });
}


// ------------------------------------------------------------


static async Task<string> GetAIResponseAsync(
    ChatClient client,
    List<ChatMessage> messages)
{
    ChatCompletion completion =
        await client.CompleteChatAsync(
            messages);

    if (completion.Content == null ||
        completion.Content.Count == 0)
    {
        return string.Empty;
    }

    return completion.Content[0].Text;
}


// ------------------------------------------------------------


static void AddAssistantMessage(
    string conversationId,
    string aiResponse,
    List<ChatMessage> messages,
    List<ConversationMessage> savedMessages)
{
    // Add to OpenAI conversation
    messages.Add(
        ChatMessage.CreateAssistantMessage(
            aiResponse));


    // Add to persistent conversation
    savedMessages.Add(
        new ConversationMessage
        {
            ConversationId = conversationId,
            Role = "assistant",
            Content = aiResponse,
            CreatedDate = DateTime.Now
        });
}


// ------------------------------------------------------------

