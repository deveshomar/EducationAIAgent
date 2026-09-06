using MultiAgentOrchUsingSementicKernal.Agents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiAgentOrchUsingSementicKernal.Router
{

    public class AgentExecuter
    {
        private readonly PayrollAgent _payrollAgent;
        private readonly LeaveAgent _leaveAgent;
        private readonly ServiceNowAgent _snAgent;

        public AgentExecuter()
        {
            _payrollAgent = new PayrollAgent();
            _leaveAgent = new LeaveAgent();
            _snAgent = new ServiceNowAgent();   
        }

        public async Task<List<string>> ExecuteAsync(List<string> agents, string question)
        {
            var tasks = new List<Task<string>>();

            foreach (var agent in agents)
            {
                switch (agent.Trim().ToLower())
                {
                    case "payroll":
                        tasks.Add(_payrollAgent.ExecuteAsync(question));
                        break;

                    case "leave":
                        tasks.Add(_leaveAgent.ExecuteAsync(question));
                        break;

                        case "servicenowhelpdesk":
                    tasks.Add(_snAgent.ExecuteAsync(question));
                    break;

                }
            }

            var responses = await Task.WhenAll(tasks);

            return responses.ToList();
        }
    }

}
