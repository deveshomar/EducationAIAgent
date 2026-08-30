using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sementickernalEmpAgent.Tools.Payroll
{
    public class PayrollPlugin
    {
        [KernelFunction]
        [Description("Get salary information for an employee")]
        public string GetSalary(
            [Description("Employee ID")] int employeeId)
        {
            Console.WriteLine($"TOOL: GetSalary({employeeId})");

            return """
        {
            "employeeId": 101,
            "monthlySalary": 150000,
            "currency": "INR"
        }
        """;
        }

        [KernelFunction]
        [Description("Get the latest salary slip for an employee")]
        public string GetSalarySlip(
            [Description("Employee ID")] int employeeId)
        {
            Console.WriteLine($"TOOL: GetSalarySlip({employeeId})");

            return """
        {
            "employeeId": 101,
            "month": "July 2026",
            "grossSalary": 150000,
            "netSalary": 125000
        }
        """;
        }
    }
}
