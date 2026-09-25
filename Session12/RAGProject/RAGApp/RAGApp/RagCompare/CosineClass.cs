using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAGApp.RagCompare
{
    public class CosineClass
    {
      public  static double CosineSimilarity(float[] vectorA, float[] vectorB)
        {
            double dotProduct = 0;
            double magnitudeA = 0;
            double magnitudeB = 0;

            for (int i = 0; i < vectorA.Length; i++)
            {
                dotProduct += vectorA[i] * vectorB[i];

                magnitudeA += vectorA[i] * vectorA[i];

                magnitudeB += vectorB[i] * vectorB[i];
            }

            return dotProduct /
                (Math.Sqrt(magnitudeA) * Math.Sqrt(magnitudeB));
        }
    }
}
