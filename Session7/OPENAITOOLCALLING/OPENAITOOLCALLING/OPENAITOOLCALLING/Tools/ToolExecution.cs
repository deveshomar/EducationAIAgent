using EmployeeAssistant;
using OpenAI.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OPENAITOOLCALLING.Tools
{
    public class ToolExecution
    {
        private readonly EmployeeTools _employeeTools;
        public  ToolExecution(EmployeeTools employeeTools)
        {
            _employeeTools = employeeTools;
        }   
        public  string ExecuteTool(ChatToolCall toolCall,List<string > toolresult)
        {
            string toolName = toolCall.FunctionName;

            using JsonDocument arguments =
                JsonDocument.Parse(toolCall.FunctionArguments);

            switch (toolName)
            {
                case "get_salary_details":
                    {
                        int employeeId =
                            arguments.RootElement
                                .GetProperty("employeeId")
                                .GetInt32();

                        return _employeeTools
                            .GetSalaryDetails(employeeId);
                    }

                case "get_leave_balance":
                    {
                        int employeeId =
                            arguments.RootElement
                                .GetProperty("employeeId")
                                .GetInt32();

                        return _employeeTools
                            .GetLeaveBalance(employeeId);
                    }

                case "apply_leave":
                    {
                        int employeeId =
                            arguments.RootElement
                                .GetProperty("employeeId")
                                .GetInt32();

                        string leaveType =
                            arguments.RootElement
                                .GetProperty("leaveType")
                                .GetString()!;

                        int numberOfDays =
                            arguments.RootElement
                                .GetProperty("numberOfDays")
                                .GetInt32();

                        return _employeeTools
                            .ApplyLeave(
                                employeeId,
                                leaveType,
                                numberOfDays);
                    }

                case "get_leave_history":
                    {
                        int employeeId =
                            arguments.RootElement
                                .GetProperty("employeeId")
                                .GetInt32();

                        return _employeeTools
                            .GetLeaveHistory(employeeId);
                    }

                case "get_tax_details":
                    {
                        int employeeId =
                            arguments.RootElement
                                .GetProperty("employeeId")
                                .GetInt32();

                        return _employeeTools
                            .GetTaxDetails(employeeId);
                    }

                case "get_employee_details":
                    {
                        int employeeId =
                            arguments.RootElement
                                .GetProperty("employeeId")
                                .GetInt32();

                        return _employeeTools
                            .GetEmployeeDetails(employeeId);
                    }

                case "get_attendance_details":
                    {
                        int employeeId =
                            arguments.RootElement
                                .GetProperty("employeeId")
                                .GetInt32();

                        return _employeeTools
                            .GetAttendanceDetails(employeeId);
                    }

                case "get_manager_details":
                    {
                        int employeeId =
                            arguments.RootElement
                                .GetProperty("employeeId")
                                .GetInt32();

                        return _employeeTools
                            .GetManagerDetails(employeeId);
                    }

                case "send_email":
                    {
                        string to =
                            arguments.RootElement
                                .GetProperty("to")
                                .GetString()!;

                        return _employeeTools.SendEmail(to, toolresult);
                    }

                default:
                    return $"Unknown tool: {toolName}";
            }
        }
    }
}
