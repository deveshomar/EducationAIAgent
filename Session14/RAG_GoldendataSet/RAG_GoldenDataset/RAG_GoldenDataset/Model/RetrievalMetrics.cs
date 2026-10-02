using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAG_GoldenDataset.Model
{
    public class RetrievalMetrics
    {
        public double PrecisionAtK { get; set; }

        public double RecallAtK { get; set; }

        public double HitRateAtK { get; set; }

        public double ReciprocalRank { get; set; }
    }
}
