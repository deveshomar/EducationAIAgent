using OpenAI.Chat;
using System.Text.Json;

namespace EmployeeAssistant;

public static class ToolDefinitions
{
    public static List<ChatTool> GetTools()
    {
        return new List<ChatTool>
        {
            GetSalaryDetailsTool(),
            GetLeaveBalanceTool(),
            ApplyLeaveTool(),
            GetLeaveHistoryTool(),
            GetTaxDetailsTool(),
            GetEmployeeDetailsTool(),
            GetAttendanceDetailsTool(),
            GetManagerDetailsTool(),
            SendEmailTool()
        };
    }

    private static ChatTool GetSalaryDetailsTool()
    {
        return ChatTool.CreateFunctionTool(
            functionName: "get_salary_details",
            functionDescription:
                "Get salary and payroll details for an employee.",
            functionParameters: BinaryData.FromString("""
            {
                "type": "object",
                "properties": {
                    "employeeId": {
                        "type": "integer",
                        "description": "Employee ID"
                    }
                },
                "required": ["employeeId"]
            }
            """)
        );
    }

    private static ChatTool GetLeaveBalanceTool()
    {
        return ChatTool.CreateFunctionTool(
            functionName: "get_leave_balance",
            functionDescription:
                "Get the current leave balance of an employee including casual, sick and earned leaves.",
            functionParameters: BinaryData.FromString("""
            {
                "type": "object",
                "properties": {
                    "employeeId": {
                        "type": "integer",
                        "description": "Employee ID"
                    }
                },
                "required": ["employeeId"]
            }
            """)
        );
    }

    private static ChatTool ApplyLeaveTool()
    {
        return ChatTool.CreateFunctionTool(
            functionName: "apply_leave",
            functionDescription:
                "Apply leave for an employee.  we need for to check balance of leave if emp have  so we need to call get_leave_balance ",
            functionParameters: BinaryData.FromString("""
            {
                "type": "object",
                "properties": {
                    "employeeId": {
                        "type": "integer"
                    },
                    "leaveType": {
                        "type": "string",
                        "description": "Type of leave such as Casual, Sick or Earned"
                    },
                    "numberOfDays": {
                        "type": "integer"
                    }
                },
                "required": [
                    "employeeId",
                    "leaveType",
                    "numberOfDays"
                ]
            }
            """)
        );
    }

    private static ChatTool GetLeaveHistoryTool()
    {
        return ChatTool.CreateFunctionTool(
            functionName: "get_leave_history",
            functionDescription:
                "Get the historical leave records of an employee.",
            functionParameters: BinaryData.FromString("""
            {
                "type": "object",
                "properties": {
                    "employeeId": {
                        "type": "integer"
                    }
                },
                "required": ["employeeId"]
            }
            """)
        );
    }

    private static ChatTool GetTaxDetailsTool()
    {
        return ChatTool.CreateFunctionTool(
            functionName: "get_tax_details",
            functionDescription:
                "Get income tax and tax deduction details for an employee.",
            functionParameters: BinaryData.FromString("""
            {
                "type": "object",
                "properties": {
                    "employeeId": {
                        "type": "integer"
                    }
                },
                "required": ["employeeId"]
            }
            """)
        );
    }

    private static ChatTool GetEmployeeDetailsTool()
    {
        return ChatTool.CreateFunctionTool(
            functionName: "get_employee_details",
            functionDescription:
                "Get basic employee information.",
            functionParameters: BinaryData.FromString("""
            {
                "type": "object",
                "properties": {
                    "employeeId": {
                        "type": "integer"
                    }
                },
                "required": ["employeeId"]
            }
            """)
        );
    }

    private static ChatTool GetAttendanceDetailsTool()
    {
        return ChatTool.CreateFunctionTool(
            functionName: "get_attendance_details",
            functionDescription:
                "Get attendance information for an employee.",
            functionParameters: BinaryData.FromString("""
            {
                "type": "object",
                "properties": {
                    "employeeId": {
                        "type": "integer"
                    }
                },
                "required": ["employeeId"]
            }
            """)
        );
    }

    private static ChatTool GetManagerDetailsTool()
    {
        return ChatTool.CreateFunctionTool(
            functionName: "get_manager_details",
            functionDescription:
                "Get manager information for an employee.",
            functionParameters: BinaryData.FromString("""
            {
                "type": "object",
                "properties": {
                    "employeeId": {
                        "type": "integer"
                    }
                },
                "required": ["employeeId"]
            }
            """)
        );
    }

    private static ChatTool SendEmailTool()
    {
        return ChatTool.CreateFunctionTool(
            functionName: "send_email",
            functionDescription:
                "Send an email to the specified email address. " +
                "Use this only when the user explicitly asks to email or send information by email.",
            functionParameters: BinaryData.FromString("""
            {
                "type": "object",
                "properties": {
                    "to": {
                        "type": "string",
                        "description": "Recipient email address"
                    }
                },
                "required": ["to"]
            }
            """)
        );
    }
}