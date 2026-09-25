using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfImprovingAgent.Model
{
    using SelfImprovingAgent.OpenAIService;
    using System.Text.Json;

    public class PortfolioEvaluator
    {
        private readonly LlmService _llm;

        public PortfolioEvaluator(LlmService llm)
        {
            _llm = llm;
        }

        public async Task<EvaluationResult> EvaluateAsync(
            InvestmentRequest request,
            Portfolio portfolio,
            string research)
        {
            var portfolioJson =
                JsonSerializer.Serialize(
                    portfolio,
                    new JsonSerializerOptions
                    {
                        WriteIndented = true
                    });

            var prompt = $$"""
        You are an independent Portfolio Evaluator.

        IMPORTANT:
        You are NOT the Portfolio Planner.

        Your job is to critically evaluate the
        proposed portfolio.

        USER REQUIREMENTS
        -----------------

        Investment Amount:
        ₹{{request.Amount:N0}}

        Horizon:
        {{request.HorizonYears}} years

        Risk Profile:
        {{request.RiskProfile}}

        Required Liquidity:
        ₹{{request.LiquidityRequired:N0}}

        Objective:
        {{request.Objective}}


        RESEARCH
        --------

        {{research}}


        PROPOSED PORTFOLIO
        ------------------

        {{portfolioJson}}


        Evaluate the portfolio using these criteria:

        Goal Alignment       = 20 points
        Risk Management      = 20 points
        Diversification      = 20 points
        Liquidity             = 15 points
        Research Quality      = 15 points
        Scenario Analysis     = 10 points

        TOTAL = 100 points.


        Be critical.

        Do not give a high score simply because
        the portfolio looks reasonable.

        Identify weaknesses that another agent
        can improve.

        Return ONLY valid JSON:

        {
          "score": 0,
          "gaps": [],
          "improvementActions": [],
          "feedback": ""
        }
        """;

            var json =
                await _llm.AskAsync(prompt);

            return JsonSerializer.Deserialize<EvaluationResult>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })!;
        }
    }
}
