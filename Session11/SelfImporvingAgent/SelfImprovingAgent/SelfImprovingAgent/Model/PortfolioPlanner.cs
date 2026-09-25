using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfImprovingAgent.Model
{
    using SelfImprovingAgent.OpenAIService;
    using System.Text.Json;

    public class PortfolioPlanner
    {
        private readonly LlmService _llm;

        public PortfolioPlanner(LlmService llm)
        {
            _llm = llm;
        }

        public async Task<Portfolio> CreateAsync(
            InvestmentRequest request,
            string research,
            Portfolio? previousPortfolio,
            EvaluationResult? previousEvaluation)
        {
            string previousPortfolioText =
                previousPortfolio == null
                    ? "No previous portfolio."
                    : JsonSerializer.Serialize(
                        previousPortfolio);

            string previousEvaluationText =
                previousEvaluation == null
                    ? "No previous evaluation."
                    : JsonSerializer.Serialize(
                        previousEvaluation);

            var prompt = $$"""
    You are a Portfolio Planning Agent.

    USER REQUIREMENTS
    -----------------

    Amount:
    ₹{{request.Amount:N0}}

    Horizon:
    {{request.HorizonYears}} years

    Risk:
    {{request.RiskProfile}}

    Liquidity:
    ₹{{request.LiquidityRequired:N0}}

    Objective:
    {{request.Objective}}


    CURRENT RESEARCH
    ----------------

    {{research}}


    PREVIOUS PORTFOLIO
    ------------------

    {{previousPortfolioText}}


    PREVIOUS EVALUATION
    -------------------

    {{previousEvaluationText}}


    Create a portfolio proposal.

    If a previous portfolio exists:

    - Do not blindly repeat it.
    - Study the evaluator feedback.
    - Address the identified gaps.
    - Improve the portfolio where appropriate.
    - Explain why changes were made.

    Return ONLY valid JSON:

    {
      "equityPercent": 0,
      "debtPercent": 0,
      "goldPercent": 0,
      "liquidPercent": 0,
      "rationale": ""
    }

    Allocation percentages must total exactly 100.
    """;

            var json =
                await _llm.AskAsync(prompt);

            return JsonSerializer.Deserialize<Portfolio>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })!;
        }
    }
}
