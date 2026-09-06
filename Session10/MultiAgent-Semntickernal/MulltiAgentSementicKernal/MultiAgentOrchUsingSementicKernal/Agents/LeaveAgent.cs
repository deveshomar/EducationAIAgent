using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using MultiAgentOrchUsingSementicKernal.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiAgentOrchUsingSementicKernal.Agents
{
    
    
    public class LeaveAgent : BaseAgent
    {
        public LeaveAgent()
            : base(KernelFactory.CreateLeaveKernel())
        {
        }

        public override async Task<string> ExecuteAsync(string question)
        {
            var prompt = $"""
            You are Leave Management Agent.

            Always use available functions whenever required.

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

            return result.ToString();
        }
    }
}
