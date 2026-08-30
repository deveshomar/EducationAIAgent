using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sementickernalEmpAgent.Tools.ITHelpDESK
{
    public class ITPlugin
    {
        [KernelFunction]
        [Description("Create an IT helpdesk ticket for an employee")]
        public string CreateITTicket(
        [Description("Employee ID")] int employeeId,
        [Description("Description of the IT problem")] string issue)
        {
            Console.WriteLine(
                $"TOOL: CreateITTicket({employeeId})");

            return """
        {
            "ticketId": "IT-10025",
            "status": "Created",
            "priority": "Medium",
            "message": "IT ticket created successfully."
        }
        """;
        }


        [KernelFunction]
        [Description("This function contains steps to fix password issues , this function need to call when user facing password issues ")]
        public string PasswordIssues(
       
        [Description("Description of the IT problem")] string issue)
        {
            Console.WriteLine(
                $"TOOL: PasswordIssues()");

              return "These are steps to follow step 1, step 2, step 3  and if still not work call to helpdesk at 101011112345";

        
        }
    }
}
