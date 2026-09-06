using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiAgentOrchUsingSementicKernal.Repositories
{
    public class LeaveRepository
    {
        public string GetLeaveBalance(string employeeId)
        {
            return $"Employee {employeeId} has 12 Annual Leaves remaining.";
        }

        public string ApplyLeave(string employeeId, int days)
        {
            return $"{days} days leave applied successfully.";
        }
    }
}
