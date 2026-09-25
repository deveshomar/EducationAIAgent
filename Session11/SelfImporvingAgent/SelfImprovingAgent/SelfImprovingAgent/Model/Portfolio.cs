using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfImprovingAgent.Model
{
    public class Portfolio
    {
        public decimal EquityPercent { get; set; }

        public decimal DebtPercent { get; set; }

        public decimal GoldPercent { get; set; }

        public decimal LiquidPercent { get; set; }

        public string Rationale { get; set; } = "";
    }
}
