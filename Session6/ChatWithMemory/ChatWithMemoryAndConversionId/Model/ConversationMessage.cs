using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatWithMemoryAndConversionId.Model
{
    public class ConversationMessage
    {
        public string ConversationId { get; set; }

        //Who sent the message: "user" or "assistant"
        public string Role { get; set; }

        //AI response or user message content
        public string Content { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
