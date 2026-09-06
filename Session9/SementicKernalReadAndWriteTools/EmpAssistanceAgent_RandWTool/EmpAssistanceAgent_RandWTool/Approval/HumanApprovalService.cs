using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpAssistanceAgent_RandWTool.Approval
{
    public class HumanApprovalService
    {
        public bool RequestApproval(
            string employeeId,
            string action,
            Dictionary<string, string> arguments)
        {
            Console.WriteLine();
            Console.WriteLine("======================================");
            Console.WriteLine("       HUMAN APPROVAL REQUIRED");
            Console.WriteLine("======================================");

            Console.WriteLine($"Employee : {employeeId}");
            Console.WriteLine($"Action   : {action}");

            Console.WriteLine();
            Console.WriteLine("Arguments:");

            foreach (var item in arguments)
            {
                Console.WriteLine(
                    $"  {item.Key} = {item.Value}");
            }

            Console.WriteLine();
            Console.Write("Do you want to APPROVE? (Y/N): ");

            var answer = Console.ReadLine();

            return answer?.Equals(
                "Y",
                StringComparison.OrdinalIgnoreCase) == true;
        }
    }
}
