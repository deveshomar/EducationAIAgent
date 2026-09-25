using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfImprovingAgent.Model
{
    public class EvaluationResult
    {
        public int Score { get; set; }

        public List<string> Gaps { get; set; } = [];

        public List<string> ImprovementActions { get; set; } = [];

        public string Feedback { get; set; } = "";
    }
}
