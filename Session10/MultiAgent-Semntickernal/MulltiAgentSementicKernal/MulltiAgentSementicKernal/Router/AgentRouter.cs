using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SemanticKernel;
using MulltiAgentSementicKernal.AGENTS;

namespace MulltiAgentSementicKernal.Router
{
    

    public class AgentRouter
    {
        private readonly PayrollAgent payroll;
        private readonly LeaveAgent leave;
        private readonly HrAgent hr;
        private readonly ItAgent it;

        public AgentRouter(Kernel kernel)
        {
            payroll = new PayrollAgent(kernel);
            leave = new LeaveAgent(kernel);
            hr = new HrAgent(kernel);
            it = new ItAgent(kernel);
        }

        public async Task<List<string>> ExecuteAsync(List<string> agents,
            string question)
        {
            var tasks = new List<Task<string>>();

            foreach (var agent in agents)
            {
                switch (agent.ToLower())
                {
                    case "payroll":
                        tasks.Add(payroll.ExecuteAsync(question));
                        break;

                    case "leave":
                        tasks.Add(leave.ExecuteAsync(question));
                        break;

                    case "hr":
                        tasks.Add(hr.ExecuteAsync(question));
                        break;

                    case "it":
                        tasks.Add(it.ExecuteAsync(question));
                        break;
                }
            }

            return (await Task.WhenAll(tasks)).ToList();
        }
    }
}
