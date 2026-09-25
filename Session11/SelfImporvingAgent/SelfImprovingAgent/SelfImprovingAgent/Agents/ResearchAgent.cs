using SelfImprovingAgent.Model;
using SelfImprovingAgent.OpenAIService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfImprovingAgent.Agents
{
    public class ResearchAgent
    {
        private readonly LlmService _llm;

        public ResearchAgent(LlmService llm)
        {
            _llm = llm;
        }

        public async Task<string> ResearchAsync(
            InvestmentRequest request,
            List<string> gaps)
        {
            string gapText =
                gaps.Count == 0
                    ? "No previous evaluation exists. Perform broad research."
                    : string.Join("\n", gaps);

            var prompt = $$"""
        You are an Investment Research Agent.

        USER REQUIREMENTS
        -----------------

        Amount:
        ₹{{request.Amount:N0}}

        Horizon:
        {{request.HorizonYears}} years

        Risk Profile:
        {{request.RiskProfile}}

        Required Liquidity:
        ₹{{request.LiquidityRequired:N0}}

        Objective:
        {{request.Objective}}


        PREVIOUS EVALUATION GAPS
        ------------------------

        {{gapText}}


        YOUR TASK
        ---------

        Perform research that helps the Portfolio Planner
        create a better portfolio.

        If there are no previous gaps:
        - Perform broad research.

        If previous gaps exist:
        - Focus specifically on those gaps.
        - Find information that can help address them.


        Cover:

        1. Suitable asset classes
        2. Risk considerations
        3. Diversification
        4. Liquidity
        5. Investment horizon
        6. Downside scenarios
        7. Important assumptions


        IMPORTANT:

        Do NOT create the final portfolio.

        Return research findings that can be consumed
        by the Portfolio Planner.
        """;

            return await _llm.AskAsync(prompt);
        }
    }
}
