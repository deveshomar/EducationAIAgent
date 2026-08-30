using Microsoft.SemanticKernel;
using System.ComponentModel;

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
}