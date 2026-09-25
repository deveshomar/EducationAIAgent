using SelfImprovingAgent.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SelfImprovingAgent.Agents
{
    using SelfImprovingAgent.OpenAIService;
    using System.Text.Json;

    public class RequirementAgent
    {
        private readonly LlmService _llm;

        public RequirementAgent(LlmService llm)
        {
            _llm = llm;
        }

        public async Task<InvestmentRequest> ExtractAsync(
            string userMessage,
            InvestmentRequest? existingRequest = null)
        {
            string existingJson =
                existingRequest == null
                    ? "No previous information."
                    : JsonSerializer.Serialize(existingRequest);

            var prompt = $$"""
        You are an Investment Requirement Extraction Agent.

        Existing requirements:

        {{existingJson}}

        User's latest message:

        {{userMessage}}

        Extract investment requirements.

        Fields:

        - amount
        - horizonYears
        - riskProfile
        - liquidityRequired
        - objective

        Rules:

        1. Keep information from the existing requirements.
        2. Update fields when the user provides new information.
        3. Do not remove existing information unless the user explicitly changes it.
        4. Do not invent missing information.
        5. Convert crore/lakh amounts into rupees.
        6. Return ONLY valid JSON.
        7. consider liquidity of Rs 3000000 as a minimum requirement for liquidity if user not provied

        Example:

        {
          "amount": 20000000,
          "horizonYears": 5,
          "riskProfile": "Moderate",
          "liquidityRequired": 2000000,
          "objective": "Reasonable growth with controlled risk"
        }
        """;

            var json = await _llm.AskAsync(prompt);

            return JsonSerializer.Deserialize<InvestmentRequest>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })!;
        }
    }
}
