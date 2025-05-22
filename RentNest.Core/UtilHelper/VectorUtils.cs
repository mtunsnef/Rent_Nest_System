using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Core.UtilHelper
{
    public static class VectorUtils
    {
        public static float CosineSimilarity(List<float> a, List<float> b)
        {
            if (a.Count != b.Count)
                throw new ArgumentException("Vectors must be the same length.");

            float dot = 0f, normA = 0f, normB = 0f;

            for (int i = 0; i < a.Count; i++)
            {
                dot += a[i] * b[i];
                normA += a[i] * a[i];
                normB += b[i] * b[i];
            }

            return dot / ((float)Math.Sqrt(normA) * (float)Math.Sqrt(normB));
        }
    }

}
