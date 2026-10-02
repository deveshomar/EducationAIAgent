using RAG_GoldenDataset.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAG_GoldenDataset.Services
{
    public static class RagEvaluationService
    {
        public static RetrievalMetrics Calculate(
            List<string> retrievedIds,
            List<string> relevantIds,
            int k)
        {
            var topK = retrievedIds
                .Take(k)
                .ToList();

            int relevantRetrieved =
                topK.Count(id => relevantIds.Contains(id));

            double precision =
                (double)relevantRetrieved / k;

            double recall =
                relevantIds.Count == 0
                    ? 0
                    : (double)relevantRetrieved /
                      relevantIds.Count;

            double hitRate =
                relevantRetrieved > 0 ? 1 : 0;

            double reciprocalRank = 0;

            for (int i = 0; i < topK.Count; i++)
            {
                if (relevantIds.Contains(topK[i]))
                {
                    reciprocalRank =
                        1.0 / (i + 1);

                    break;
                }
            }

            return new RetrievalMetrics
            {
                PrecisionAtK = precision,
                RecallAtK = recall,
                HitRateAtK = hitRate,
                ReciprocalRank = reciprocalRank
            };
        }
    }
}
