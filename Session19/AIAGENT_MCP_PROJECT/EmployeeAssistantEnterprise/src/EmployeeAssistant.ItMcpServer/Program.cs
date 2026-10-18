
using System.ComponentModel;
using System.Net.Http.Json;
using ModelContextProtocol.Server;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient("ItApi", client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["ItApi:BaseUrl"]
        ?? "http://localhost:5036/");

    client.Timeout = TimeSpan.FromSeconds(10);
});

builder.Services
    .AddMcpServer()
    .WithHttpTransport()
    .WithTools<ItTools>();

var app = builder.Build();

app.MapMcp("/mcp");

app.MapGet("/health", () => Results.Ok(new
{
    Service = "EmployeeAssistant.ItMcpServer",
    Status = "Healthy"
}));

app.Run();

[McpServerToolType]
public sealed class ItTools(IHttpClientFactory httpClientFactory)
{
    private readonly HttpClient _http =
        httpClientFactory.CreateClient("ItApi");

    [McpServerTool(Name = "get_network_diagnostics")]
    [Description("Read network diagnostic information for an authorized employee.")]
    public async Task<string> GetNetworkDiagnostics(
        [Description("Employee ID resolved by trusted identity context")] string employeeId)
    {
        return await GetApiResult(
            $"api/it/employees/{Uri.EscapeDataString(employeeId)}/network-diagnostics");
    }

    [McpServerTool(Name = "get_device_details")]
    [Description("Retrieve device details for an authorized employee.")]
    public async Task<string> GetDeviceDetails(
        [Description("Employee ID resolved by trusted identity context")] string employeeId)
    {
        return await GetApiResult(
            $"api/it/employees/{Uri.EscapeDataString(employeeId)}/device");
    }

    [McpServerTool(Name = "search_it_knowledge")]
    [Description("Search the IT knowledge base for read-only troubleshooting guidance.")]
    public async Task<string> SearchItKnowledge(
        [Description("Search query, such as VPN, DNS or Wi-Fi")] string query)
    {
        if (string.IsNullOrWhiteSpace(query) || query.Length > 200)
        {
            return """{"error":"Query must contain 1 to 200 characters"}""";
        }

        return await GetApiResult(
            $"api/it/knowledge?query={Uri.EscapeDataString(query)}");
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
            return """{"error":"IT API is unavailable"}""";
        }
        catch (TaskCanceledException)
        {
            return """{"error":"IT API request timed out"}""";
        }
    }
}
