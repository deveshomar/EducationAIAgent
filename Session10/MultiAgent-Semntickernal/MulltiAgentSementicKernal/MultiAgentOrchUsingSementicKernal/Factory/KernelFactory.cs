using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SemanticKernel;
using MultiAgentOrchUsingSementicKernal.Plugins;


namespace MultiAgentOrchUsingSementicKernal.Factory
{

    public static class KernelFactory
    {
        private const string ApiKey = "";

       // private const string ApiKey = "YOUR_OPENAI_KEY";
        private const string Model = "gpt-4.1-mini";

        public static Kernel CreateSupervisorKernel()
        {
            var builder = Kernel.CreateBuilder();

            builder.AddOpenAIChatCompletion(Model, ApiKey);

            return builder.Build();
        }

        public static Kernel CreatePayrollKernel()
        {
            var builder = Kernel.CreateBuilder();

            builder.AddOpenAIChatCompletion(Model, ApiKey);

            builder.Plugins.AddFromType<PayrollPlugin>();

            return builder.Build();
        }

        public static Kernel CreateLeaveKernel()
        {
            var builder = Kernel.CreateBuilder();

            builder.AddOpenAIChatCompletion(Model, ApiKey);

            builder.Plugins.AddFromType<LeavePlugin>();

            return builder.Build();
        }



        public static Kernel CreateSNKernal()
        {
            var builder = Kernel.CreateBuilder();

            builder.AddOpenAIChatCompletion(Model, ApiKey);

            builder.Plugins.AddFromType<ServiceNowPlugin>();

            return builder.Build();
        }
    }
}
/*
 * using Microsoft.SemanticKernel;

namespace MultiAgentOrchUsingSementicKernal.Factory
{
    public static class KernelFactory
    {
        private const string Model = "gpt-4.1-mini";

        public static Kernel CreateKernel()
        {
            var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new InvalidOperationException(
                    "OPENAI_API_KEY environment variable is not configured.");
            }

            var builder = Kernel.CreateBuilder();

            builder.AddOpenAIChatCompletion(
                modelId: Model,
                apiKey: apiKey);

            return builder.Build();
        }

        public static Kernel CreateAgentKernel<TPlugin>()
            where TPlugin : class, new()
        {
            var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new InvalidOperationException(
                    "OPENAI_API_KEY environment variable is not configured.");
            }

            var builder = Kernel.CreateBuilder();

            builder.AddOpenAIChatCompletion(
                modelId: Model,
                apiKey: apiKey);

            builder.Plugins.AddFromType<TPlugin>();

            return builder.Build();
        }
    }
}


var payrollKernel =
    KernelFactory.CreateAgentKernel<PayrollPlugin>();

var leaveKernel =
    KernelFactory.CreateAgentKernel<LeavePlugin>();

var supervisor =
    KernelFactory.CreateKernel();
 */ 