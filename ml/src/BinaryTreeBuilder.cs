using System;
using System.Collections.Generic;

namespace mlAssignment1
{
    public class BinaryTreeBuilder
    {
        private List<DataRow> _training;

        private BooleanTreeNode _tree;

        public BinaryTreeBuilder(List<DataRow> trainingData)
        {
            _training = trainingData;
            // special root node with no attributes
            _tree = new BooleanTreeNode("Root");

            if (trainingData.Count > 0)
            {
                HashSet<string> attributes = new HashSet<string>(trainingData[1].Attributes);

                BuildTree(_tree, _training, attributes);
            }
        }

        private void BuildTree(BooleanTreeNode node, List<DataRow> subset, HashSet<string> remainingAttributes)
        {
            // The meat of the algorithm

            /*

             LearnTree(X,Y)
                – Input:
                • Set X of R training vectors, each containing the values (x1,..,xM) of M attributes (X1,..,XM)
                • A vector Y of R elements, where yj = class of the jth datapoint
                – If all the datapoints in X have the same class value y
                    • Return a leaf node that predicts y as output
                – If all the datapoints in X have the same attribute value (x1,..,xM)
                    • Return a leaf node that predicts the majority of the class values in Y
                as output
                – Try all the possible attributes Xj and choose the one, j*, for which IG(Y|Xj) is maximum
                    – For every possible value v of Xj*:
                        – Xv,Yv= set of datapoints for which xj* = v and corresponding classes
                        – Child <= LearnTree(Xv, Yv)

            */
            int pureClass = -1;
            if(IsSubsetPure(subset, out pureClass))
            {
                // excellent, we have a pure node!
                // set the classification and return
                node.Classification = pureClass;
                return;
            }

            if(remainingAttributes.Count == 0)
            {
                // we have nothing else to splice, just return the majority or the global majority
                node.Classification = MajorClass(subset);
                return;
            }

            // get the entropy of the current node and subset
            double parentEntropy = Entropy(subset);

            // calculate the most information gained
            foreach(string attr in remainingAttributes)
            {
                // in a binary tree node, there will only be two results when splitting
                // by an attribute

                //MathHelpers.InformationGained()
            }
        }

        private bool IsSubsetPure(List<DataRow> subset, out int firstClassification)
        {
            firstClassification = -1;
            for (int i = 0; i < subset.Count; i++)
            {
                DataRow current = subset[i];

                int classification = current.RetrieveClassification();
                if (firstClassification == -1)
                {
                    firstClassification = classification;
                    continue;
                }

                if (classification != firstClassification)
                {
                    return false;
                }
            }

            return true;
        }

        private int MajorClass(List<DataRow> subset)
        {
            Dictionary<int, int> majorities = CountByClassification(subset);

            // TODO: Pick the overall majority in case of tie. This just picks the first one
            int currentMax = 0;
            int maxClass = 0;
            foreach(var kvp in majorities)
            {
                if (kvp.Value > currentMax)
                {
                    currentMax = kvp.Value;
                    maxClass = kvp.Key;
                }
            }

            return maxClass;
        }

        private double Entropy(List<DataRow> subset)
        {
            Dictionary<int, int> counts = CountByClassification(subset);

            List<double> dists = new List<double>();

            foreach(var kvp in counts)
            {
                double fraction = (double)kvp.Value / (double)subset.Count;
                dists.Add(fraction);
            }

            return MathHelpers.Entropy(dists.ToArray());
        }

        private Dictionary<int, int> CountByClassification(List<DataRow> subset)
        {
            return CountByAttribute(subset, "classification");
        }

        private Dictionary<int, int> CountByAttribute(List<DataRow> subset, string key)
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

        private double[,] GetEntropiesAndDistByAttr(List<DataRow> subset, string attr)
        {
            // this method will be a bit specialized since we are in the binary tree builder
            Dictionary<int, List<DataRow>> split = new Dictionary<int, List<DataRow>>();
            Dictionary<int, int> distrobutionByAttr = new Dictionary<int, int>();

            for(int i = 0; i < subset.Count; i++)
            {
                DataRow current = subset[i];
                int result = current.RetrieveValueAsInt(attr); 
                if(split.ContainsKey(result))
                {
                    split[result].Add(current);
                }
                else
                {
                    List<DataRow> subsubset = new List<DataRow>();
                    subsubset.Add(current);
                    split.Add(result, subsubset);
                }
            }

            foreach(var kvp in split)
            {
                distrobutionByAttr.Add(kvp.Key, kvp.Value.Count);
            }

            // x is first or seconc array
            // y is either entropy or distro

            double[,] @return = new double[2, distrobutionByAttr.Count];

            for (int x = 0; x < 2; x++)
            {
                // todo: formalize which count you should use

                // get all the entropies of subset
                for(int y = 0; y < split.Count; y++)
                {
                    @return[x, y]
                }
            }

            //return @return;
            return new double[0, 0];
        }

        /// <summary>
        /// Will classify and print how accurate the built decision tree is
        /// </summary>
        /// <param name="testData"></param>
        public void Classify(List<DataRow> testData)
        {

        }
    }

    public static class DataRowExtensions
    {
        public static int RetrieveClassification(this DataRow row)
        {
            return row.RetrieveValueAsInt("classification");
        }
    }
}
