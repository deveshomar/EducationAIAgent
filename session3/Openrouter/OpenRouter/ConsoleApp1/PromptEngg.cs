using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRouter
{
    public   static class Prompttemplate
    {

        public const string codingPrompt = @"Write a C# .NET 8 method that takes a list of integers and returns the top 5
unique numbers in descending order. Handle null and empty input.
Explain the time and space complexity.";

        public const string simplePrompt = "Please write 100 words story for kids";

        public const string ComplexPrompt = @"Design a multi-agent enterprise system using C# and .NET.

Agents:
- HR Agent
- Payroll Agent
- Leave Agent
- IT Helpdesk Agent
- Finance Agent

Create a supervisor/router agent that receives a user request and decides which
agent should handle it.

Support:
- Parallel agent execution
- Tool calling
- Model routing
- Conversation memory
- RAG
- Human approval
- Retry
- Fallback
- Observability

Explain the complete request flow with an example:
""How many leaves do I have and what is my current salary?""";
    }
}
