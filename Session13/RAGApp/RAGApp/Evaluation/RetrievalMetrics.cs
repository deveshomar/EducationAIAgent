using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAGApp_testCases.Evaluation
{
    public static class RetrievalMetrics
    {
        public static double HitRateAtK(
    IEnumerable<int> retrieved,
    IEnumerable<int> relevant)
        {
            return retrieved.Intersect(relevant).Any()
                ? 1
                : 0;
        }
        public static double PrecisionAtK(
  IEnumerable<int> retrieved,
  IEnumerable<int> relevant)
        {
            var retrievedList = retrieved.ToList();

            if (retrievedList.Count == 0)
                return 0;

            int relevantRetrieved =
                retrievedList.Intersect(relevant).Count();

            return (double)relevantRetrieved /
                   retrievedList.Count;
        }
        public static double RecallAtK(
    IEnumerable<int> retrieved,
    IEnumerable<int> relevant)
        {
            var relevantList = relevant.ToList();

            if (relevantList.Count == 0)
                return 0;

            int relevantRetrieved =
                retrieved.Intersect(relevantList).Count();

            return (double)relevantRetrieved /
                   relevantList.Count;
        }
        public static double ReciprocalRank(
    IList<int> retrieved,
    IEnumerable<int> relevant)
        {
            var relevantSet = relevant.ToHashSet();

            for (int i = 0; i < retrieved.Count; i++)
            {
                if (relevantSet.Contains(retrieved[i]))
                    return 1.0 / (i + 1);
            }

            return 0;
        }
        public static double NdcgAtK(
    IList<int> retrieved,
    IEnumerable<int> relevant)
        {
            var relevantSet = relevant.ToHashSet();

            double dcg = 0;

            for (int i = 0; i < retrieved.Count; i++)
            {
                if (relevantSet.Contains(retrieved[i]))
                {
                    dcg += 1.0 / Math.Log2(i + 2);
                }
            }

            int idealRelevant = Math.Min(relevantSet.Count, retrieved.Count);

            double idcg = 0;

            for (int i = 0; i < idealRelevant; i++)
            {
                idcg += 1.0 / Math.Log2(i + 2);
            }

            if (idcg == 0)
                return 0;

            return dcg / idcg;
        }

    }



}

