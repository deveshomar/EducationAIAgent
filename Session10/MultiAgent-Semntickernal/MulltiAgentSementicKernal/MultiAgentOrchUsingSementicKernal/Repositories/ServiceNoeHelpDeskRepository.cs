using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiAgentOrchUsingSementicKernal.Repositories
{
    public  class ServiceNowHelpDeskRepository
    {
        public string LaptopIssues(string employeeId)
        {
            return $"Employee {employeeId} Laptop issues is not fixed";
        }
        public string AccessIssues(string employeeId)
        {
            return $"Employee {employeeId} Access issues is  fixed";
        }
        public string PasswordIssues(string employeeId)
        {
            return "For all password issues call Toll free at 19093039 or Do Step1 step2 step 3";
        }

    }
}
