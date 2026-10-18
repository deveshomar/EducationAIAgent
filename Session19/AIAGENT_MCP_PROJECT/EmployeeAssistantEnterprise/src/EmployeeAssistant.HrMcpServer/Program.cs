
using System.ComponentModel;
using System.Net.Http.Json;
using ModelContextProtocol.Server;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient("HrApi", client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["HrApi:BaseUrl"]
        ?? "http://localhost:5192/");

    client.Timeout = TimeSpan.FromSeconds(10);
});

builder.Services
    .AddMcpServer()
    .WithHttpTransport()
    .WithTools<HrTools>();

var app = builder.Build();

app.MapMcp("/mcp");

app.MapGet("/health", () => Results.Ok(new
{
    Service = "EmployeeAssistant.HrMcpServer",
    Status = "Healthy"
}));

app.Run();

[McpServerToolType]
public sealed class HrTools(IHttpClientFactory httpClientFactory)
{
    private readonly HttpClient _http =
        httpClientFactory.CreateClient("HrApi");

    [McpServerTool(Name = "get_my_leave_balance")]
    [Description("Read an employee's leave balance from the HR API. The employee ID must come from trusted authenticated identity context, not from an untrusted chat message.")]
    public async Task<string> GetLeaveBalance(
        [Description("Employee ID resolved by the trusted application")] string employeeId)
    {
        return await GetApiResult(
            $"api/hr/employees/{Uri.EscapeDataString(employeeId)}/leave-balance");
    }

    [McpServerTool(Name = "get_my_employee_profile")]
    [Description("Retrieve an employee profile from the HR API.")]
    public async Task<string> GetEmployeeProfile(
        [Description("Employee ID resolved by the trusted application")] string employeeId)
    {
        return await GetApiResult(
            $"api/hr/employees/{Uri.EscapeDataString(employeeId)}/profile");
    }

    [McpServerTool(Name = "get_my_salary_slip")]
    [Description("Retrieve salary slip metadata for an employee and month in YYYY-MM format. Read-only operation.")]
    public async Task<string> GetSalarySlip(
        [Description("Employee ID resolved by the trusted application")] string employeeId,
        [Description("Salary month in YYYY-MM format")] string month)
    {
        if (!System.Text.RegularExpressions.Regex.IsMatch(
                month, @"^\d{4}-(0[1-9]|1[0-2])$"))
        {
            return """{"error":"Month must use YYYY-MM format"}""";
        }

        return await GetApiResult(
            $"api/hr/employees/{Uri.EscapeDataString(employeeId)}/salary-slips/{Uri.EscapeDataString(month)}");
    }

    private async Task<string> GetApiResult(string path)
    {
        try
        {
            using var response = await _http.GetAsync(path);
            var body = await response.Content.ReadAsStringAsync();

            return System.Text.Json.JsonSerializer.Serialize(new
            {
                StatusCode = (int)response.StatusCode,
                Success = response.IsSuccessStatusCode,
                Data = body
            });
        }
        catch (HttpRequestException)
        {
            return """{"error":"HR API is unavailable"}""";
        }
        catch (TaskCanceledException)
        {
            return """{"error":"HR API request timed out"}""";
        }
    }
}
