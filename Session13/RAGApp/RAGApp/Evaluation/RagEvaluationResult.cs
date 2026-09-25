using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAGApp_testCases.Evaluation
{
    public class RagEvaluationResult
    {
        public string Question { get; set; } = string.Empty;

        public List<int> RetrievedChunkIds { get; set; } = [];

        public List<int> RelevantChunkIds { get; set; } = [];

        public double HitRate { get; set; }

        public double Precision { get; set; }

        public double Recall { get; set; }

        public double MRR { get; set; }

        public double NDCG { get; set; }
    }
}
