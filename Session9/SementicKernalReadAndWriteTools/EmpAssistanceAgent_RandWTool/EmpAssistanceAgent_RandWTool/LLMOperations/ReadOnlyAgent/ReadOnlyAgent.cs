using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpAssistanceAgent_RandWTool.LLMOperations.ReadOnlyAgent
{
    public class ReadOnlyAgent
    {
        private readonly Kernel _kernel;

        public ReadOnlyAgent(Kernel kernel)
        {
            _kernel = kernel;
        }

        public async Task<string> MakeLLMCall(string userRequest)
        {
            var settings = new OpenAIPromptExecutionSettings
            {
                FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
            };

            var result = await _kernel.InvokePromptAsync(
                userRequest,
                new KernelArguments(settings));

            return result.ToString();
        }
    }
}
