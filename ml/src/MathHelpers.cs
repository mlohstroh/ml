using System;

namespace mlAssignment1
{
    public static class MathHelpers
    {
        /// <summary>
        /// Calculates the entropy for a set of distrubtions. Lower entropy is best
        /// </summary>
        /// <param name="distributions"></param>
        /// <returns></returns>
        public static double Entropy(params double[] distributions)
        {
            double sum = 0.0f;
            for (int i = 0; i < distributions.Length; i++)
            {
                // assume 0log2(0) == 0
                if (distributions[i] == 0)
                    continue;
                sum += -distributions[i] * Math.Log(distributions[i], 2);
            }

            return sum;
        }

        /// <summary>
        /// Calculates the infomation gained between a parent node and splitting based a specific attribute
        /// Note, the arrays must be ordered so they match up. 
        /// </summary>
        /// <param name="parentEntropy"></param>
        /// <param name="childEntropies"></param>
        /// <param name="childDistributions"></param>
        /// <returns></returns>
        public static double InformationGained(double parentEntropy, double[] childEntropies, double[] childDistributions)
        {
            double rightSum = 0.0;

            if (childEntropies.Length != childDistributions.Length)
                throw new Exception("Arrays are not equal. Cannot calculate information gained");

            for(int i = 0; i < childEntropies.Length; i++)
            {
                rightSum += childEntropies[i] * childDistributions[i];
            }

            return parentEntropy - rightSum;
        }
    }
}
