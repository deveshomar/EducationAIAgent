using OPENAITOOLCALLING.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeAssistant;

public class EmployeeTools
{
    public string GetSalaryDetails(int employeeId)
    {
        Console.WriteLine("Making DB call for GetSalaryDetails..");

        // Dummy implementation
        return $"Salary details for employee {employeeId}: " +
               "Basic Salary: 80000, HRA: 20000, Allowances: 10000";
    }

    public string GetLeaveBalance(int employeeId)
    {
        Console.WriteLine("Making DB call for GetLeaveBalance..");

        // Dummy implementation
        return $"Leave balance for employee {employeeId}: " +
               "Casual Leave: 5, Sick Leave: 7, Earned Leave: 12";
    }

    public string ApplyLeave(int employeeId, string leaveType, int numberOfDays)
    {
        

        Console.WriteLine("Making DB call for ApplyLeave..");

        // Dummy implementation
        return $"Leave application created for employee {employeeId}. " +
               $"Type: {leaveType}, Days: {numberOfDays}";
    }

    public string GetLeaveHistory(int employeeId)
    {
        Console.WriteLine("Making DB call for GetLeaveHistory..");

        // Dummy implementation
        return $"Leave history for employee {employeeId}: " +
               "01-Aug-2026 to 02-Aug-2026 - Casual Leave; " +
               "10-Jul-2026 - Sick Leave";
    }

    public string GetTaxDetails(int employeeId)
    {
        Console.WriteLine("Making DB call for GetTaxDetails..");

        // Dummy implementation
        return $"Tax details for employee {employeeId}: " +
               "Annual Taxable Income: 950000, Tax Deducted: 85000";
    }

    public string GetEmployeeDetails(int employeeId)
    {
        Console.WriteLine("Making DB call for GetEmployeeDetails..");

        // Dummy implementation
        return $"Employee {employeeId}: Devesh Omar, " +
               "Department: IT, Designation: Tech Lead";
    }

    public string GetAttendanceDetails(int employeeId)
    {
        Console.WriteLine("Making DB call for GetAttendanceDetails..");

        // Dummy implementation
        return $"Attendance details for employee {employeeId}: " +
               "Present: 20 days, Absent: 1 day, WFH: 3 days";
    }

    public string GetManagerDetails(int employeeId)
    {
        Console.WriteLine("Making DB call for GetManagerDetails..");
        // Dummy implementation
        return $"Manager details for employee {employeeId}: " +
               "Manager: Amit Sharma, Department: IT";
    }

    // ----------------------------------------------------
    // EMAIL METHOD
    // ----------------------------------------------------
    // You will implement email sending later.
    // Only the recipient email is passed.
    public string SendEmail(string to, List<string> responseTool)
    {
        Console.WriteLine("Sending Email..");
        string commandText = string.Join(", ", responseTool);
        EmailService.SendEmail(to, commandText);
        Console.WriteLine("Sending Email..");
        // TODO:
        // Add your email implementation here later.

        return $"Email request prepared for {to}";
    }
}
