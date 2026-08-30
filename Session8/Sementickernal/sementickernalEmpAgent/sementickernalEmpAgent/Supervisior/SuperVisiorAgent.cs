using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sementickernalEmpAgent.Supervisior
{
    public class SuperVisiorAgent
    {
        private readonly Kernel _kernel;

        public SuperVisiorAgent(string apiKey)
        {
            var builder = Kernel.CreateBuilder();

            builder.AddOpenAIChatCompletion(
                modelId: "gpt-4.1-mini",
                apiKey: apiKey);

            _kernel = builder.Build();
        }

        public async Task<string> RouteAsync(string userQuery)
        {
            var prompt = $"""
        You are a Supervisor Agent.

        Your job is to identify which specialist agent
        should handle the user's request.

        Available agents:

       

        LEAVE
        - Leave balance
        - Leave history
        - Apply leave
        - Cancel leave

        PAYROLL
        - Salary
        - Salary slip

        IT
        - IT helpdesk
        - IT ticket
        - IT support    
        - Password reset
        - Software installation
        - Hardware issues
        - Network problems
        - password issues
        - 

        Rules:

        1. Return a JSON array only.
        2. The array can contain one or multiple agents.
        3. If the request involves leave, return "LEAVE".
        4. If the request involves payroll or salary, return "PAYROLL".
        5. If the request involves IT support, return "IT".
        6. If the request contains multiple topics, return all relevant agents.
        7. If no agent is relevant, return ["UNKNOWN"].
        8. Agent names must be uppercase.
        9. Do not return any explanation or additional text.

        Valid examples:

        User: "What is my leave balance?"
        Output:
        ["LEAVE"]

        User: "Show my salary slip"
        Output:
        ["PAYROLL"]

        User: "My laptop is not working"
        Output:
        ["IT"]

        User: "Show my leave balance and salary slip"
        Output:
        ["LEAVE", "PAYROLL"]

        User: "I need my leave balance and my laptop has a network problem"
        Output:
        ["LEAVE", "IT"]

        User: "Tell me the weather"
        Output:
        ["UNKNOWN"]

        User may ask a quesiton about leave, payroll, or IT support.
        single quesiton may contain multiple queries, you need to identify the most relevant agent for the user query.
        please reponin JSON format only, do not provide any explanation or additional text, just return the agent name or "UNKNOWN".
      

        User request:
        {userQuery}

        if you are not able to identify the agent, return "UNKNOWN"
        please do not provide any explanation or additional text, just return the agent name or "UNKNOWN".
        please return the result in uppercase letters.
        please do not return any other text or explanation, just the agent name or "UNKNOWN".


        """;

            var result = await _kernel.InvokePromptAsync(prompt);

            return result.ToString().Trim();
        }
    }
}
