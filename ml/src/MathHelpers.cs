using System;
using System.Collections.Generic;

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

        public static double Entropy(List<DataRow> subset)
        {
            Dictionary<int, int> counts = CountByClassification(subset);

            List<double> dists = new List<double>();

            foreach (var kvp in counts)
            {
                double fraction = (double)kvp.Value / (double)subset.Count;
                dists.Add(fraction);
            }

            return Entropy(dists.ToArray());
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

        public static double InformationGained(BooleanTreeNode parent, BooleanTreeNode left, BooleanTreeNode right)
        {
            int totalRows = parent.Subset.Count;

            double sub = 0.0;
            sub += left.GetEntropy() * (left.Subset.Count / totalRows);
            sub += right.GetEntropy() * (right.Subset.Count / totalRows);

            return parent.GetEntropy() - sub;
        }

        public static Dictionary<int, int> CountByClassification(List<DataRow> subset)
        {
            return CountByAttribute(subset, "classification");
        }

        public static Dictionary<int, int> CountByAttribute(List<DataRow> subset, string key)
        {
            Dictionary<int, int> counts = new Dictionary<int, int>();
            for (int i = 0; i < subset.Count; i++)
            {
                DataRow current = subset[i];
                int classification = current.RetrieveValueAsInt(key);
                // increment via dict
                if (counts.ContainsKey(classification))
                {
                    counts[classification] = counts[classification] + 1;
                }
                else
                {
                    counts[classification] = 1;
                }
            }
            return counts;
        }
    }
}
