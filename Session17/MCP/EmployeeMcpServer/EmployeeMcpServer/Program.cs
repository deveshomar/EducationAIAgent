
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using System.ComponentModel;

var builder = Host.CreateApplicationBuilder(args);

// MCP uses stdout for protocol messages.
// Keep application logs on stderr.
builder.Logging.AddConsole(options =>
{
    options.LogToStandardErrorThreshold = LogLevel.Trace;
});

builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithTools<EmployeeTools>();

Console.WriteLine("Employee MCP Server is running. Press Ctrl+C to exit.");
await builder.Build().RunAsync();




[McpServerToolType]
public sealed class EmployeeTools
{
    [McpServerTool, Description(
        "Gets basic employee details using an employee ID.")]
    public static object GetEmployeeDetails(
        [Description("Employee ID, for example EMP101")]
        string employeeId)
    {
        if (employeeId.Equals(
            "EMP101", StringComparison.OrdinalIgnoreCase))
        {
            return new
            {
                EmployeeId = "EMP101",
                Name = "Rahul Sharma",
                Department = "IT",
                Designation = "Software Engineer"
            };
        }


        if (employeeId.Equals(
           "EMP102", StringComparison.OrdinalIgnoreCase))
        {
            return new
            {
                EmployeeId = "EMP102",
                Name = "Pawan Sharma",
                Department = "Manager",
                Designation = "Software Engineer"
            };
        }

        return new
        {
            Error = "Employee not found",
            EmployeeId = employeeId
        };
    }

    [McpServerTool, Description(
        "Gets the remaining annual leave balance for an employee.")]
    public static object GetLeaveBalance(
        [Description("Employee ID, for example EMP101")]
        string employeeId)
    {
        if (employeeId.Equals(
            "EMP101", StringComparison.OrdinalIgnoreCase))
        {
            return new
            {
                EmployeeId = "EMP101",
                AnnualLeaveBalance = 12,
                Unit = "days"
            };
        }


        if (employeeId.Equals(
           "EMP102", StringComparison.OrdinalIgnoreCase))
        {
            return new
            {
                EmployeeId = "EMP102",
                AnnualLeaveBalance = 31,
                Unit = "days"
            };
        }

        return new
        {
            Error = "Employee not found",
            EmployeeId = employeeId
        };
    }
}
