using ChatWithMemoryAndConversionId.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChatWithMemoryAndConversionId.FILEIO
{
    public class FileOperations
    {
        // SAVE METHOD
       public static async Task SaveMessages(
            string fileName,
            List<ConversationMessage> messages)
        {
            string json =
                JsonSerializer.Serialize(
                    messages,
                    new JsonSerializerOptions
                    {
                        WriteIndented = true
                    });

            await File.WriteAllTextAsync(fileName, json);
        }

      public  static async Task SaveConversationAsync( string fileName,
            List<ConversationMessage> savedMessages)
        {
            await FileOperations.SaveMessages(
                fileName,
                savedMessages);
        }

        public static async Task<List<ConversationMessage>>  LoadConversationAsync(string fileName)
        {
            if (!File.Exists(fileName))
            {
                Console.WriteLine(
                    "Conversation file does not exist.");

                return new List<ConversationMessage>();
            }

            string oldJson =
                await File.ReadAllTextAsync(fileName);

            List<ConversationMessage>? messages =
                JsonSerializer.Deserialize<
                    List<ConversationMessage>>(oldJson);

            Console.WriteLine(
                "Old conversation loaded.");

            return messages ?? new List<ConversationMessage>();
        }
    }
}
