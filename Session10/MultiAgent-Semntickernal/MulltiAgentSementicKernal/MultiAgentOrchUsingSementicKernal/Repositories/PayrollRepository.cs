using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiAgentOrchUsingSementicKernal.Repositories
{
   
    public class PayrollRepository
    {
        public string GetSalary(string employeeId)
        {
            return $"Employee {employeeId} Salary : ₹80,000";
        }

        public string GetPayrollDate()
        {
            return "Salary will be credited on 30th of every month.";
        }
    }
}
