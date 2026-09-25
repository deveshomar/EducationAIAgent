using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfImprovingAgent.Model
{
    public class InvestmentRequest
    {
        public decimal? Amount { get; set; }

        public int? HorizonYears { get; set; }

        public string? RiskProfile { get; set; }

        public decimal? LiquidityRequired { get; set; }

        public string? Objective { get; set; }
    }
}
