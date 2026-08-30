using Microsoft.SemanticKernel;
using System.ComponentModel;

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
}