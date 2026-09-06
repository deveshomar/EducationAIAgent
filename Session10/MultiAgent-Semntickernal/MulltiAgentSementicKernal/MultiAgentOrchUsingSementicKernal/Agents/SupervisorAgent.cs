using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SemanticKernel;
using MultiAgentOrchUsingSementicKernal.Factory;


namespace MultiAgentOrchUsingSementicKernal.Agents
{

    public class SupervisorAgent : BaseAgent
    {
        public SupervisorAgent()
            : base(KernelFactory.CreateSupervisorKernel())
        {
        }

        public async Task<List<string>> DecideAsync(string question)
        {
            var prompt = $"""
You are a routing agent.

Available agents:

Payroll
Leave
ServiceNowHelpDesk

Return only comma separated names.

Question:

{question}
""";

            var result = await Kernel.InvokePromptAsync(prompt);

            return result.ToString()
                         .Split(',')
                         .Select(x => x.Trim())
                         .ToList();
        }

        public override Task<string> ExecuteAsync(string question)
            => throw new NotImplementedException();
    }
}
