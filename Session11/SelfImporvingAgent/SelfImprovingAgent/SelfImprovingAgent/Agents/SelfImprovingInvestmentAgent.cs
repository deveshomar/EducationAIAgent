using SelfImprovingAgent.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfImprovingAgent.Agents
{
    public class SelfImprovingInvestmentAgent
    {
        private readonly ResearchAgent _researchAgent;
        private readonly PortfolioPlanner _planner;
        private readonly PortfolioEvaluator _evaluator;

        public SelfImprovingInvestmentAgent(
            ResearchAgent researchAgent,
            PortfolioPlanner planner,
            PortfolioEvaluator evaluator)
        {
            _researchAgent = researchAgent;
            _planner = planner;
            _evaluator = evaluator;
        }

        public async Task<Portfolio> RunAsync(
            InvestmentRequest request,
            int targetScore = 90,
            int maxIterations = 5)
        {
            Portfolio? previousPortfolio = null;

            EvaluationResult? previousEvaluation = null;

            List<string> gaps = [];

            Portfolio? bestPortfolio = null;

            int bestScore = 0;


            for (int iteration = 1;
                 iteration <= maxIterations;
                 iteration++)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"========== ITERATION {iteration} ==========");


                // =========================================
                // STEP 1: Research
                // =========================================

                Console.WriteLine();
                Console.WriteLine("Researching...");

                var research =
                    await _researchAgent.ResearchAsync(
                        request,
                        gaps);


                // =========================================
                // STEP 2: Create / Improve Portfolio
                // =========================================

                Console.WriteLine(
                    "Creating portfolio...");

                var portfolio =
                    await _planner.CreateAsync(
                        request,
                        research,
                        previousPortfolio,
                        previousEvaluation);


                // =========================================
                // STEP 3: Evaluate
                // =========================================

                Console.WriteLine(
                    "Evaluating portfolio...");

                var evaluation =
                    await _evaluator.EvaluateAsync(
                        request,
                        portfolio,
                        research);


                // =========================================
                // STEP 4: Display Score
                // =========================================

                Console.WriteLine();
                Console.WriteLine(
                    $"Score: {evaluation.Score}/100");

                Console.WriteLine(
                    $"Feedback: {evaluation.Feedback}");


                // =========================================
                // STEP 5: Track Best Portfolio
                // =========================================

                if (evaluation.Score > bestScore)
                {
                    bestScore =
                        evaluation.Score;

                    bestPortfolio =
                        portfolio;
                }


                // =========================================
                // STEP 6: Target Reached?
                // =========================================

                if (evaluation.Score >= targetScore)
                {
                    Console.WriteLine();
                    Console.WriteLine(
                        "Target score reached!");

                    return portfolio;
                }


                // =========================================
                // STEP 7: Get Gaps
                // =========================================

                Console.WriteLine();
                Console.WriteLine("Gaps:");

                foreach (var gap in evaluation.Gaps)
                {
                    Console.WriteLine(
                        $"- {gap}");
                }


                // =========================================
                // STEP 8: Prepare Next Iteration
                // =========================================

                gaps =
                    evaluation.Gaps;

                previousPortfolio =
                    portfolio;

                previousEvaluation =
                    evaluation;
            }


            // =============================================
            // Maximum iterations reached
            // =============================================

            Console.WriteLine();
            Console.WriteLine(
                $"Maximum iterations reached.");

            Console.WriteLine(
                $"Best score: {bestScore}/100");

            return bestPortfolio!;
        }
    }
}
