using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SemanticKernel;

namespace MulltiAgentSementicKernal.AGENTS
{
    
    public class LeaveAgent
    {
        private readonly Kernel _kernel;

        public LeaveAgent(Kernel kernel)
        {
            _kernel = kernel;
        }

        public async Task<string> ExecuteAsync(string question)
        {
            var prompt = $"""
You are Leave Management Agent.

Answer only leave related queries.

Question:

{question}
""";

            return (await _kernel.InvokePromptAsync(prompt)).ToString();
        }
    }
}
