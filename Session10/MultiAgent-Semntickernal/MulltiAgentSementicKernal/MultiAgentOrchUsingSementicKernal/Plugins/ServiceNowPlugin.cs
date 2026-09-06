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
    public class ServiceNowPlugin
    {
        private readonly ServiceNowHelpDeskRepository repository;

        public ServiceNowPlugin()
        {
            repository = new ServiceNowHelpDeskRepository();
        }


        [KernelFunction]
        [Description("Retrieves the laptop or computer-related issues reported by an employee. Use this function when the employee asks about their laptop problems, computer issues, or previously reported laptop/IT hardware issues.")]
        public string LaptopIssues(
      [Description("The unique employee ID used to identify the employee whose laptop issues should be retrieved.")]
    string employeeId)
        {
            return repository.LaptopIssues(employeeId);
        }

        [KernelFunction]
        [Description("Retrieves password-related issues reported by a specific employee. Use this function when an employee asks about password problems, password errors, password reset issues, or other issues related to their account password.")]
        public string PasswordIssues(
     [Description("Unique employee ID used to identify the employee whose password issues need to be retrieved.")]
    string employeeId)
        {
            return repository.PasswordIssues(employeeId);
        }


        [KernelFunction]
        [Description("Retrieves system or application access issues reported by a specific employee. Use this function when an employee cannot access an application, system, resource, or other company service, or asks about their reported access problems.")]
        public string AccessIssues(
            [Description("Unique employee ID used to identify the employee whose access issues need to be retrieved.")]
    string employeeId)
        {
            return repository.AccessIssues(employeeId);
        }

    }
}
