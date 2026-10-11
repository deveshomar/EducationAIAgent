
using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol;

// 1. Configure how the client launches the MCP server.
var transport = new StdioClientTransport(
    new StdioClientTransportOptions
    {
        Name = "Employee MCP Server",
        Command = "C:\\Program Files\\dotnet\\dotnet.exe",
        Arguments =
        [
            "run",
            "--project",
            @"D:\Sessions\Education\Session17\MCP\EmployeeMcpServer\EmployeeMcpServer\EmployeeMcpServer.csproj"
        ]
    });

// 2. Connect to the server.
await using var client = await McpClient.CreateAsync(transport);

Console.WriteLine("Connected to Employee MCP Server!");
Console.WriteLine();

// 3. Discover the tools exposed by the server.
var tools = await client.ListToolsAsync();

Console.WriteLine("Available MCP tools:");

foreach (var tool in tools)
{
    Console.WriteLine($"- {tool.Name}: {tool.Description}");
}

Console.WriteLine();

// 4. Invoke GetEmployeeDetails.
var employeeResult = await client.CallToolAsync(
    "get_employee_details",
    new Dictionary<string, object?>
    {
        ["employeeId"] = "EMP101"
    });



//-get_leave_balance: Gets the remaining annual leave balance for an employee.
//- get_employee_details: Gets basic employee details using an employee ID.

Console.WriteLine("Employee Details:");
foreach (var item in employeeResult.Content)
{
    if (item is ModelContextProtocol.Protocol.TextContentBlock text)
    {
        Console.WriteLine(text.Text);
    }
}

Console.WriteLine();

// 5. Invoke GetLeaveBalance.
var leaveResult = await client.CallToolAsync(
    "get_leave_balance",
    new Dictionary<string, object?>
    {
        ["employeeId"] = "EMP101"
    });

Console.WriteLine("Leave Balance:");
foreach (var item in leaveResult.Content)
{
    if (item is ModelContextProtocol.Protocol.TextContentBlock text)
    {
        Console.WriteLine(text.Text);
    }
}
