using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatWithMemoryAndConversionId
{
    public  class Helper
    {

        public static string GetLast10messageFromChatHistory()
        {


        }
       public static string GetConversationId()
        {
            Console.Write("Enter Conversation Id: ");

            string? conversationId = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(conversationId))
            {
                conversationId = Guid.NewGuid().ToString();

                Console.WriteLine(
                    $"New Conversation Created: {conversationId}");
            }

            return conversationId;
        }
    }
}
