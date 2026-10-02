using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAG_GoldenDataset.Util
{
  
    public static class CosineSimilarity
    {
        public static double Calculate(
            float[] a,
            float[] b)
        {
            double dot = 0;
            double magnitudeA = 0;
            double magnitudeB = 0;

            for (int i = 0; i < a.Length; i++)
            {
                dot += a[i] * b[i];

                magnitudeA += a[i] * a[i];

                magnitudeB += b[i] * b[i];
            }

            if (magnitudeA == 0 || magnitudeB == 0)
                return 0;

            return dot /
                (Math.Sqrt(magnitudeA) *
                 Math.Sqrt(magnitudeB));
        }
    }
}
