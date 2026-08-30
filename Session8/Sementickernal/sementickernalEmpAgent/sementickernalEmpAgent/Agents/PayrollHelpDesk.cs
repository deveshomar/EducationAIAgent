using Microsoft.SemanticKernel;
using sementickernalEmpAgent.Tools.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sementickernalEmpAgent.Agents
{
    public static class PayrollHelpDesk
    {
        public static Kernel CreatePayrollAgent(string apiKey)
        {
            var builder = Kernel.CreateBuilder();

            builder.AddOpenAIChatCompletion(
                modelId: "gpt-4.1-mini",
                apiKey: apiKey);

            builder.Plugins.AddFromType<PayrollPlugin>();

            return builder.Build();
        }
    }
}
