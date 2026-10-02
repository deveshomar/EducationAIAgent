using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplePolicyRag.Eval
{
    public class RagMetrics
    {
        public double ContextRelevance { get; set; }
        public double ContextPrecision { get; set; }
        public double ContextRecall { get; set; }
        public double AnswerRelevance { get; set; }
        public double Faithfulness { get; set; }
    }
}
