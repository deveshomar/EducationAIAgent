using Microsoft.SemanticKernel;
using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace EmpAIAgent_MCP
{
    public class McpEmployeePlugin
    {
        private readonly McpClient _mcpClient;

        public McpEmployeePlugin(McpClient mcpClient)
        {
            _mcpClient = mcpClient;
        }

        [KernelFunction("get_employee_details")]
        [Description(
            "Gets employee details using an employee ID. " +
            "Use this to find the employee name, department, and designation.")]
        public async Task<string> GetEmployeeDetailsAsync(
            [Description("Employee ID, such as EMP101 or EMP102")]
        string employeeId)
        {
            return await CallMcpToolAsync(
                "get_employee_details", employeeId);
        }

        [KernelFunction("get_leave_balance")]
        [Description(
            "Gets the remaining annual leave balance for an employee. " +
            "Use this when the user asks about remaining leave days.")]
        public async Task<string> GetLeaveBalanceAsync(
            [Description("Employee ID, such as EMP101 or EMP102")]
        string employeeId)
        {
            return await CallMcpToolAsync(
                "get_leave_balance", employeeId);
        }

        private async Task<string> CallMcpToolAsync(
            string toolName,
            string employeeId)
        {
            var result = await _mcpClient.CallToolAsync(
                toolName,
                new Dictionary<string, object?>
                {
                    ["employeeId"] = employeeId
                });

            return string.Join(
                "\n",
                result.Content
                    .OfType<TextContentBlock>()
                    .Select(block => block.Text));
        }
    }
}
