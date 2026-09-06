
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace TaskWhenAllDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var stopwatch = Stopwatch.StartNew();

            Console.WriteLine("Calling APIs in parallel...");

            // Start all tasks immediately
            Task<string> employeeTask = GetEmployeeAsync();
            Task<string> salaryTask = GetSalaryAsync();
            Task<string> leaveTask = GetLeaveBalanceAsync();
            Task<string> projectTask = GetProjectDetailsAsync();

            // Wait for all tasks to complete
            await Task.WhenAll(
                employeeTask,
                salaryTask,
                leaveTask,
                projectTask);

            Console.WriteLine();
            Console.WriteLine("Results:");
            Console.WriteLine(employeeTask.Result);
            Console.WriteLine(salaryTask.Result);
            Console.WriteLine(leaveTask.Result);
            Console.WriteLine(projectTask.Result);

            stopwatch.Stop();

            Console.WriteLine();
            Console.WriteLine($"Total Time: {stopwatch.ElapsedMilliseconds} ms");
        }

        static async Task<string> GetEmployeeAsync()
        {
            Console.WriteLine("Fetching Employee Details...");
            await    Task.Delay(2000);
            return "Employee: John";
        }

        static async Task<string> GetSalaryAsync()
        {
            Console.WriteLine("Fetching Salary Details...");
            await Task.Delay(5000);
            return "Salary: ₹100000";
        }

        static async Task<string> GetLeaveBalanceAsync()
        {
            Console.WriteLine("Fetching Leave Balance...");
            await Task.Delay(9000);
            return "Leaves Available: 15";
        }

        static async Task<string> GetProjectDetailsAsync()
        {
            Console.WriteLine("Fetching Project Details...");
            await Task.Delay(2000);
            return "Project: AI Portal";
        }
    }
}