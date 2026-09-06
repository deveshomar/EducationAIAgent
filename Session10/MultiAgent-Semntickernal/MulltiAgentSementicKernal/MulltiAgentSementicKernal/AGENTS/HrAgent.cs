using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MulltiAgentSementicKernal.AGENTS
{
    using Microsoft.SemanticKernel;

    public class HrAgent
    {
        private readonly Kernel _kernel;

        public HrAgent(Kernel kernel)
        {
            _kernel = kernel;
        }

        public async Task<string> ExecuteAsync(string question)
        {
            var prompt = $"""
You are HR Helpdesk.

Answer HR policy questions only.

Question:

{question}
""";

            return (await _kernel.InvokePromptAsync(prompt)).ToString();
        }
    }
}
