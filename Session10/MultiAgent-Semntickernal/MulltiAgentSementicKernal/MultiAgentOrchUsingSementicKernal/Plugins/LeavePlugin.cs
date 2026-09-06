using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiAgentOrchUsingSementicKernal.Plugins
{
    
    using Microsoft.SemanticKernel;
    using MultiAgentOrchUsingSementicKernal.Repositories;
    using System.ComponentModel;

    public class LeavePlugin
    {
        private readonly LeaveRepository repository;

        public LeavePlugin()
        {
            repository = new LeaveRepository();
        }

        [KernelFunction]
        [Description("This function will return employee leave balance .employeeId  is manadatory field")]

        public string GetLeaveBalance(string employeeId)
        {
            return repository.GetLeaveBalance(employeeId);
        }

        [KernelFunction]
        [Description("This function is to apply leaves for emp here employeeId and days are manadatory.")]

        public string ApplyLeave(string employeeId, int days)
        {
            return repository.ApplyLeave(employeeId, days);
        }
    }
}
