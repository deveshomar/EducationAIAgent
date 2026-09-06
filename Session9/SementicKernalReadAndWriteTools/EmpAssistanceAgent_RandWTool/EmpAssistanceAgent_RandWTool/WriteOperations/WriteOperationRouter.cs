using EmpAssistanceAgent_RandWTool.Tools.Write;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpAssistanceAgent_RandWTool.WriteOperations
{
    public class WriteOperationRouter
    {
        private readonly EmployeeWriteTools _tools;

        public WriteOperationRouter(
            EmployeeWriteTools tools)
        {
            _tools = tools;
        }

        public string Execute(
            string employeeId,
            string action,
            Dictionary<string, string> arguments)
        {
            switch (action.ToLowerInvariant())
            {
                case "apply_leave":

                    return _tools.ApplyLeave(
                        employeeId,
                        arguments["fromDate"],
                        arguments["toDate"],
                        arguments["reason"]);

                case "cancel_leave":

                    return _tools.CancelLeave(   employeeId,
                        arguments["leaveId"]);

                default:

                    throw new InvalidOperationException(
                        $"Unknown write operation: {action}");
            }
        }
    }
}
