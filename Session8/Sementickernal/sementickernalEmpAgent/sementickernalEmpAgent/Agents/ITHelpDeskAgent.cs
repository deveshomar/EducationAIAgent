using Microsoft.SemanticKernel;
using sementickernalEmpAgent.Tools.ITHelpDESK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sementickernalEmpAgent.Agents
{
    public static  class ITHelpDeskAgent
    {
    
        public static Kernel CreateITAgent(string apiKey)
        {
            var builder = Kernel.CreateBuilder();

            builder.AddOpenAIChatCompletion(
                modelId: "gpt-4.1-mini",
                apiKey: apiKey);

            builder.Plugins.AddFromType<ITPlugin>();

            return builder.Build();
        }
    }
}
