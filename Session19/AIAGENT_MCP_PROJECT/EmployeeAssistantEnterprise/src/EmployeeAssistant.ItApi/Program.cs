
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var devices = new Dictionary<string, object>
{
    ["EMP101"] = new
    {
        EmployeeId = "EMP101",
        DeviceId = "LAP-1001",
        DeviceType = "Laptop",
        OperatingSystem = "Windows 11",
        Status = "Assigned"
    },
    ["EMP102"] = new
    {
        EmployeeId = "EMP102",
        DeviceId = "LAP-1002",
        DeviceType = "Laptop",
        OperatingSystem = "Windows 11",
        Status = "Assigned"
    }
};

app.MapGet("/health", () => Results.Ok(new
{
    Service = "EmployeeAssistant.ItApi",
    Status = "Healthy"
}));

// Mock read-only network diagnostics.
// No network configuration or device changes are performed.
app.MapGet("/api/it/employees/{employeeId}/network-diagnostics",
    (string employeeId) =>
    {
        if (!devices.TryGetValue(
                employeeId.ToUpperInvariant(), out var device))
        {
            return Results.NotFound(new
            {
                Error = "Employee device not found"
            });
        }

        return Results.Ok(new
        {
            EmployeeId = employeeId.ToUpperInvariant(),
            Device = device,
            Connectivity = "Degraded",
            VpnConnected = false,
            DnsResolution = "Failed",
            RecommendedAction =
                "Check VPN connectivity and verify DNS settings.",
            DiagnosticTimestamp = DateTimeOffset.UtcNow
        });
    });

// GET: Assigned device information
app.MapGet("/api/it/employees/{employeeId}/device",
    (string employeeId) =>
    {
        if (!devices.TryGetValue(
                employeeId.ToUpperInvariant(), out var device))
        {
            return Results.NotFound(new
            {
                Error = "Employee device not found"
            });
        }

        return Results.Ok(device);
    });

// GET: Read-only IT knowledge search
app.MapGet("/api/it/knowledge",
    (string? query) =>
    {

        // htpclient client= new httpclient ("https:\\absc.ragendpint");
         // await cleint.postASYnc(querty)


        if (string.IsNullOrWhiteSpace(query))
        {
            return Results.BadRequest(new
            {
                Error = "Query is required"
            });
        }

        var articles = new[]
        {
            new
            {
                Id = "IT-001",
                Title = "VPN connection troubleshooting",
                Keywords = "vpn network remote connection",
                Instructions =
                    "Verify internet connectivity, reconnect to the approved VPN, and contact IT support if the issue persists."
            },
            new
            {
                Id = "IT-002",
                Title = "DNS troubleshooting",
                Keywords = "dns network name resolution",
                Instructions =
                    "Check the approved DNS configuration and contact IT support if name resolution continues to fail."
            },
            new
            {
                Id = "IT-003",
                Title = "Wi-Fi troubleshooting",
                Keywords = "wifi wireless internet",
                Instructions =
                    "Verify Wi-Fi is enabled, connect to the approved corporate network, and report persistent connectivity issues."
            }
        };

        var terms = query.Split(
            ' ',
            StringSplitOptions.RemoveEmptyEntries |
            StringSplitOptions.TrimEntries);

        var results = articles
            .Where(article =>
                terms.Any(term =>
                    article.Title.Contains(
                        term, StringComparison.OrdinalIgnoreCase) ||
                    article.Keywords.Contains(
                        term, StringComparison.OrdinalIgnoreCase)))
            .ToArray();

        return Results.Ok(new
        {
            Query = query,
            Count = results.Length,
            Articles = results
        });
    });

app.Run();
