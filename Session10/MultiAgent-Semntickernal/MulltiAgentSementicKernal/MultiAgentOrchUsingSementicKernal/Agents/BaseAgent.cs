using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MultiAgentOrchUsingSementicKernal.Agents
{
    using Microsoft.SemanticKernel;

    public abstract class BaseAgent
    {
        protected readonly Kernel Kernel;

        protected BaseAgent(Kernel kernel)
        {
            Kernel = kernel;
        }

        public abstract Task<string> ExecuteAsync(string question);
    }
}
