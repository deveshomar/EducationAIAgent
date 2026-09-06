using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MulltiAgentSementicKernal.AGENTS
{
    using Microsoft.SemanticKernel;

    public class ItAgent
    {
        private readonly Kernel _kernel;

        public ItAgent(Kernel kernel)
        {
            _kernel = kernel;
        }

        public async Task<string> ExecuteAsync(string question)
        {
            var prompt = $"""
You are IT Helpdesk.

Answer only IT support questions.

Question:

{question}
""";

            return (await _kernel.InvokePromptAsync(prompt)).ToString();
        }
    }
}
