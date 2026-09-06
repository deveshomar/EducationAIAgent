using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using MultiAgentOrchUsingSementicKernal.Factory;

namespace MultiAgentOrchUsingSementicKernal.Agents
{




    public class PayrollAgent : BaseAgent
    {
        public PayrollAgent()
            : base(KernelFactory.CreatePayrollKernel())
        {
        }

        public override async Task<string> ExecuteAsync(string question)
        {
            var prompt = $"""
            You are Payroll Agent.

            Always answer using available plugin functions.

            Question:

        {question}
""";

            var settings = new OpenAIPromptExecutionSettings
            {
                FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
            };

            var result = await Kernel.InvokePromptAsync(
                prompt,
                new(settings));

            return result.ToString();
        }
    }
}
