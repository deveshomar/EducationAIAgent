using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAGCosineSimilarity
{
    public static class VectorMath
    {
        public static double CosineSimilarity(float[] a, float[] b)
        {
            double dot = 0;
            double magA = 0;
            double magB = 0;

            for (int i = 0; i < a.Length; i++)
            {
                dot += a[i] * b[i];
                magA += a[i] * a[i];
                magB += b[i] * b[i];
            }

            return dot / (Math.Sqrt(magA) * Math.Sqrt(magB));
        }
    }
}
