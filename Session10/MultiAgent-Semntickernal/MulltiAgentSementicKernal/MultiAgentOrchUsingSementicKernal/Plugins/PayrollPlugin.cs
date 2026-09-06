using Microsoft.SemanticKernel;
using MultiAgentOrchUsingSementicKernal.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MultiAgentOrchUsingSementicKernal.Plugins
{
    
    public class PayrollPlugin
    {
        private readonly PayrollRepository repository;

        public PayrollPlugin()
        {
            repository = new PayrollRepository();
        }

        [KernelFunction]
        [Description("Returns salary details for an employee.")]
        public string GetSalary([Description("Employee Id")] string employeeId)
        {
            return repository.GetSalary(employeeId);
        }

        [KernelFunction]
        [Description("this must Returns payroll details .")]

        public string GetPayrollDetails()
        {
            return repository.GetPayrollDate();
        }
    }
}
