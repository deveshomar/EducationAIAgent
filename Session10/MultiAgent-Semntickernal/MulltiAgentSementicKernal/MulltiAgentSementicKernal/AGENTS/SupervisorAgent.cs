using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MulltiAgentSementicKernal.AGENTS
{
    using Microsoft.SemanticKernel;

    public class SupervisorAgent
    {
        private readonly Kernel _kernel;

        public SupervisorAgent(Kernel kernel)
        {
            _kernel = kernel;
        }

        public async Task<List<string>> DecideAgents(string question)
        {
            var prompt = $"""
You are a routing agent.

Available agents

Payroll
Leave
HR
IT

Return only comma separated names.

Question:

{question}
""";

            var result = await _kernel.InvokePromptAsync(prompt);

            return result.ToString()
                         .Split(',')
                         .Select(x => x.Trim())
                         .ToList();
        }
    }
}
