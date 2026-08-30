using Microsoft.SemanticKernel;
using System.ComponentModel;
using System.Text.Json;

public class EmployeePlugin
{
    [KernelFunction]
    [Description("Get basic employee information using employee ID")]
    public string GetEmployeeDetails(
        [Description("Employee ID")] int employeeId)
    {
        Console.WriteLine($"TOOL: GetEmployeeDetails({employeeId})");

        return """
        {
            "employeeId": 101,
            "name": "Devesh",
            "department": "IT",
            "designation": "Tech Lead"
        }
        """;
    }


    [KernelFunction]
    [Description("Get manager information for an employee")]
    public string GetManagerDetails(
        [Description("Employee ID")] int employeeId)
    {
        Console.WriteLine($"TOOL: GetManagerDetails({employeeId})");

        return """
        {
            "managerId": 201,
            "managerName": "Rajesh Kumar",
            "designation": "Engineering Manager"
        }
        """;
    }


    [KernelFunction]
    [Description("Get team members for an employee")]
    public string GetTeamMembers(
        [Description("Employee ID")] int employeeId)
    {
        Console.WriteLine($"TOOL: GetTeamMembers({employeeId})");

        return """
        [
            {
                "employeeId": 102,
                "name": "Amit"
            },
            {
                "employeeId": 103,
                "name": "Rahul"
            },
            {
                "employeeId": 104,
                "name": "Priya"
            }
        ]
        """;
    }


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


    [KernelFunction]
    [Description("Get salary information for an employee")]
    public string GetSalary(
        [Description("Employee ID")] int employeeId)
    {
        Console.WriteLine($"TOOL: GetSalary({employeeId})");

        return """
        {
            "employeeId": 101,
            "monthlySalary": 150000,
            "currency": "INR"
        }
        """;
    }


    [KernelFunction]
    [Description("Get the latest salary slip for an employee")]
    public string GetSalarySlip(
        [Description("Employee ID")] int employeeId)
    {
        Console.WriteLine($"TOOL: GetSalarySlip({employeeId})");

        return """
        {
            "employeeId": 101,
            "month": "July 2026",
            "grossSalary": 150000,
            "netSalary": 125000
        }
        """;
    }


    [KernelFunction]
    [Description("Create an IT helpdesk ticket for an employee, Also this can be sutiable when emp face an issue with laptop access , outlook, password issue")]
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
}