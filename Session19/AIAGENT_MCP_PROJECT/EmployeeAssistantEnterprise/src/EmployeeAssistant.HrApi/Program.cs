
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Mock data for development only.
// Replace with repository/database-backed services later.
var leaveBalances = new Dictionary<string, object>
{
    ["EMP101"] = new
    {
        EmployeeId = "EMP101",
        AnnualLeaveTotal = 20,
        AnnualLeaveUsed = 5,
        AnnualLeaveBalance = 15,
        SickLeaveBalance = 8
    },
    ["EMP102"] = new
    {
        EmployeeId = "EMP102",
        AnnualLeaveTotal = 20,
        AnnualLeaveUsed = 12,
        AnnualLeaveBalance = 8,
        SickLeaveBalance = 6
    }
};

var employees = new Dictionary<string, object>
{
    ["EMP101"] = new
    {
        EmployeeId = "EMP101",
        Name = "Rahul Sharma",
        Department = "IT",
        Designation = "Software Engineer",
        Manager = "Priya Verma"
    },
    ["EMP102"] = new
    {
        EmployeeId = "EMP102",
        Name = "Neha Singh",
        Department = "HR",
        Designation = "HR Specialist",
        Manager = "Amit Kumar"
    }
};

var salarySlips = new Dictionary<string, object>
{
    ["EMP101:2026-09"] = new
    {
        EmployeeId = "EMP101",
        Month = "2026-09",
        Currency = "INR",
        GrossSalary = 100000,
        Deductions = 10000,
        NetSalary = 90000,
        DocumentReference = "SAL-EMP101-202609"
    }
};

// Health endpoint
app.MapGet("/health", () => Results.Ok(new
{
    Service = "EmployeeAssistant.HrApi",
    Status = "Healthy"
}));

// GET: Employee leave balance
app.MapGet("/api/hr/employees/{employeeId}/leave-balance",
    (string employeeId) =>
    {
        if (!leaveBalances.TryGetValue(
                employeeId.ToUpperInvariant(), out var balance))
        {
            return Results.NotFound(new
            {
                Error = "Employee leave balance not found"
            });
        }

        return Results.Ok(balance);
    });

// GET: Employee profile
app.MapGet("/api/hr/employees/{employeeId}/profile",
    (string employeeId) =>
    {
        if (!employees.TryGetValue(
                employeeId.ToUpperInvariant(), out var employee))
        {
            return Results.NotFound(new
            {
                Error = "Employee profile not found"
            });
        }

        return Results.Ok(employee);
    });

// GET: Employee salary slip metadata
app.MapGet("/api/hr/employees/{employeeId}/salary-slips/{month}",
    (string employeeId, string month) =>
    {
        if (!System.Text.RegularExpressions.Regex.IsMatch(
                month, @"^\d{4}-(0[1-9]|1[0-2])$"))
        {
            return Results.BadRequest(new
            {
                Error = "Month must use YYYY-MM format"
            });
        }

        var key = $"{employeeId.ToUpperInvariant()}:{month}";

        if (!salarySlips.TryGetValue(key, out var slip))
        {
            return Results.NotFound(new
            {
                Error = "Salary slip not found"
            });
        }

        return Results.Ok(slip);
    });

app.Run();
