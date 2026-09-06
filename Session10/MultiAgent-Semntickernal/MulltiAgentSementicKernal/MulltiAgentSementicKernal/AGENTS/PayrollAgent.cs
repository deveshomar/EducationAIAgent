using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SemanticKernel;


namespace MulltiAgentSementicKernal.AGENTS
{
    
    public class PayrollAgent
    {
        private readonly Kernel _kernel;

        public PayrollAgent(Kernel kernel)
        {
            _kernel = kernel;
        }

        public async Task<string> ExecuteAsync(string question)
        {
            var prompt = $"""
You are Payroll Agent.

Answer ONLY payroll related questions.

Question:

{question}
""";

            var result = await _kernel.InvokePromptAsync(prompt);

            return result.ToString();
        }
    }
}
