using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SemanticKernel;
using System.ComponentModel;


namespace EmpAssistanceAgent_RandWTool.Tools.Read
{

    public class EmployeeReadTools
    {
        [KernelFunction("get_leave_balance")]
        [Description("Gets the employee's available leave balance.")]
        public string GetLeaveBalance(
            [Description("Employee ID")] string employeeId)
        {
            Console.WriteLine(
                $"[TOOL] GetLeaveBalance called for {employeeId}");

            // Normally call database here

            return "Employee has 12 days of leave available.";
        }

        [KernelFunction("get_leave_history")]
        [Description("Gets the employee's leave history.")]
        public string GetLeaveHistory(
            [Description("Employee ID")] string employeeId)
        {
            Console.WriteLine(
                $"[TOOL] GetLeaveHistory called for {employeeId}");

            // Normally call database here

            return "Leave history: Jan 10 Sick Leave, Feb 20 Casual Leave.";
        }
    }
}
