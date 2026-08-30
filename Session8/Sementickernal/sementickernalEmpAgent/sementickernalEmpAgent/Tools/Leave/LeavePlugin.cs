using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Text.Json;

namespace sementickernalEmpAgent.Tools.Leave
{
    public class LeavePlugin
    {
        [KernelFunction]
        [Description("Get remaining leave balance for an employee")]
        public string GetLeaveBalance(
        [Description("Employee ID")] int employeeId)
        {
            Console.WriteLine($"TOOL: GetLeaveBalance({employeeId})");

            return """
        {
            "employeeId": 101,
            "leaveBalance": 12
        }
        """;
        }


        [KernelFunction]
        [Description("Get leave history for an employee")]
        public string GetLeaveHistory(
            [Description("Employee ID")] int employeeId)
        {
            Console.WriteLine($"TOOL: GetLeaveHistory({employeeId})");

            return """
        [
            {
                "from": "2026-07-10",
                "to": "2026-07-11",
                "type": "Casual Leave",
                "status": "Approved"
            },
            {
                "from": "2026-06-15",
                "to": "2026-06-15",
                "type": "Sick Leave",
                "status": "Approved"
            }
        ]
        """;
        }


        [KernelFunction]
        [Description("Apply leave for an employee")]
        public string ApplyLeave(
            [Description("Employee ID")] int employeeId,
            [Description("Leave start date")] string fromDate,
            [Description("Leave end date")] string toDate,
            [Description("Reason for leave")] string reason)
        {
            Console.WriteLine(
                $"TOOL: ApplyLeave({employeeId}, {fromDate}, {toDate})");

            var result = new
            {
                status = "SUCCESS",
                employeeId = employeeId,
                from = fromDate,
                to = toDate,
                reason = reason,
                message = "Leave application submitted successfully."
            };
            string resultString = JsonSerializer.Serialize(result);

            return resultString;
        }


        [KernelFunction]
        [Description("Cancel an existing leave application")]
        public string CancelLeave(
            [Description("Employee ID")] int employeeId,
            [Description("Leave application ID")] int leaveId)
        {
            Console.WriteLine(
                $"TOOL: CancelLeave({employeeId}, {leaveId})");

            return """
        {
            "status": "SUCCESS",
            "message": "Leave cancelled successfully."
        }
        """;
        }
    }
}
