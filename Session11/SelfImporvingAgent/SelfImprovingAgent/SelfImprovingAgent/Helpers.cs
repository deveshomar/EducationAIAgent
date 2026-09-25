using SelfImprovingAgent.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfImprovingAgent
{
    public class Helpers
    {
        public static List<string> Validate(InvestmentRequest request)
        {
            var errors = new List<string>();

            if (request.Amount == null)
                errors.Add("Investment amount is required.");

            if (request.HorizonYears == null)
                errors.Add("Investment horizon is required.");

            if (string.IsNullOrWhiteSpace(request.RiskProfile))
                errors.Add("Risk profile is required.");

            if (request.LiquidityRequired == null)
                errors.Add("Liquidity requirement is required.");

            if (string.IsNullOrWhiteSpace(request.Objective))
                errors.Add("Investment objective is required.");

            return errors;
        }
      public  static string BuildClarificationMessage(
    List<string> errors)
        {
            var message =
                "I need a few more details before I can continue:\n";

            foreach (var error in errors)
            {
                message += $"- {error}\n";
            }

            return message;
        }

      public  static List<string> ValidatePortfolio(
    Portfolio portfolio)
        {
            var errors = new List<string>();

            decimal total =
                portfolio.EquityPercent +
                portfolio.DebtPercent +
                portfolio.GoldPercent +
                portfolio.LiquidPercent;

            if (total != 100)
            {
                errors.Add(
                    $"Portfolio allocation must total 100%. " +
                    $"Current total = {total}%");
            }

            if (portfolio.EquityPercent < 0 ||
                portfolio.DebtPercent < 0 ||
                portfolio.GoldPercent < 0 ||
                portfolio.LiquidPercent < 0)
            {
                errors.Add(
                    "Allocation percentages cannot be negative.");
            }

            return errors;
        }
    }
}
