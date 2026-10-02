using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAG_GoldenDataset.Model
{
    public class AnswerEvaluationResult
    {
        public int FaithfulnessScore { get; set; }
        public string FaithfulnessReason { get; set; } = "";

        public int RelevanceScore { get; set; }
        public string RelevanceReason { get; set; } = "";

        public int CorrectnessScore { get; set; }
        public string CorrectnessReason { get; set; } = "";
    }
}
