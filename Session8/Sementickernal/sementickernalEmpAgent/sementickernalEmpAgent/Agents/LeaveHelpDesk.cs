using Microsoft.SemanticKernel;
using sementickernalEmpAgent.Tools.Leave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sementickernalEmpAgent.Agents
{
    public  static class LeaveHelpDesk
    {
        public static Kernel CreateLeaveAgent(string apiKey)
        {
            var builder = Kernel.CreateBuilder();

            builder.AddOpenAIChatCompletion(
                modelId: "gpt-4.1-mini",
                apiKey: apiKey);

            builder.Plugins.AddFromType<LeavePlugin>();

            return builder.Build();
        }
    }
}
