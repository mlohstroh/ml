/*
    Author: Mark Lohstroh

    This file contains the algorithm for building the tree based on a list of test data.
    It is recursively called and built.

*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mlAssignment1
{
    public class BooleanTreeNode
    {
        public string Name { get; private set; }

        /// <summary>
        /// The return result of the classification. This is only applicable for leaf nodes
        /// </summary>
        public int Classification { get; private set; }
        public int MatchValue { get; set; }
        private Dictionary<BooleanTreeNode, Func<DataRow, bool>> _children = new Dictionary<BooleanTreeNode, Func<DataRow, bool>>();

        public bool IsLeaf
        {
            get { return _children.Count == 0; }
        }
        public List<DataRow> Subset { get; private set; }

        public BooleanTreeNode(string name, List<DataRow> subset)
        {
            Name = name;
            Subset = subset;
        }

        public void BuildTree(HashSet<string> remainingAttributes)
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
            int pureClass = int.MinValue;
            if (IsSubsetPure(Subset, out pureClass))
            {
                // excellent, we have a pure node!
                // set the classification and return
                Classification = pureClass;
                return;
            }

            if (remainingAttributes.Count == 0)
            {
                // we have nothing else to splice, just return the majority or the global majority
                Classification = MajorClass(Subset);
                return;
            }

            // get the entropy of the current node and subset
            double parentEntropy = MathHelpers.Entropy(Subset);

            string maxAttr = null;
            double maxIG = int.MinValue;
            Dictionary<int, List<DataRow>> splitByAttr = null;

            // calculate the most information gained
            foreach (string attr in remainingAttributes)
            {
                // in a binary tree node, there will only be two results when splitting
                // by an attribute
                var tmp = DataRow.GetDistByAttr(Subset, attr);

                // == 0
                BooleanTreeNode right = new BooleanTreeNode(maxAttr, tmp[0]);
                BooleanTreeNode left = new BooleanTreeNode(maxAttr, tmp[1]);

                double ig = MathHelpers.InformationGained(this, left, right);

                if (Program.Debug)
                    Console.WriteLine("{0} Attribute has an information gain of {1}", attr, ig);
                
                if (ig > maxIG)
                {
                    maxAttr = attr;
                    maxIG = ig;
                    splitByAttr = tmp;
                }
            }

            if(Program.Debug)
                Console.WriteLine("Picking attribute {0} for an information gained of {1}", maxAttr, maxIG);

            // this is weird, repeating, leaving in for testing purposes
            BooleanTreeNode child1 = new BooleanTreeNode(maxAttr, splitByAttr[0])
            {
                MatchValue = 0
            };
            BooleanTreeNode child2 = new BooleanTreeNode(maxAttr, splitByAttr[1])
            {
                MatchValue = 1
            };

            // == 0
            AddChild(child1, (row) =>
            {
                return row.RetrieveValueAsInt(maxAttr) == child1.MatchValue;
            });

            // == 1
            AddChild(child2, (row) =>
            {
                return row.RetrieveValueAsInt(maxAttr) == child2.MatchValue;
            });

            remainingAttributes.Remove(maxAttr);

            child1.BuildTree(new HashSet<string>(remainingAttributes));
            child2.BuildTree(new HashSet<string>(remainingAttributes));
        }

        public double GetEntropy()
        {
            return MathHelpers.Entropy(Subset);
        }

        public void AddChild(BooleanTreeNode node, Func<DataRow, bool> resultFunc)
        {
            _children.Add(node, resultFunc);
        }

        public int Classify(DataRow row)
        {
            if (IsLeaf)
            {
                return Classification;
            }

            foreach(var kvp in _children)
            {
                if(kvp.Value(row))
                {
                    return kvp.Key.Classify(row);
                }
            }
            return 0;
        }

        public Dictionary<int, int> Classify(List<DataRow> data)
        {
            Dictionary<int, int> @return = new Dictionary<int, int>();

            for(int i = 0; i < data.Count; i++)
            {
                int @class = Classify(data[i]);

                if(@return.ContainsKey(@class))
                {
                    @return[@class] = @return[@class] + 1;
                }
                else
                {
                    @return[@class] = 1;
                }
            }

            return @return;
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
            Dictionary<int, int> majorities = MathHelpers.CountByClassification(subset);

            // TODO: Pick the overall majority in case of tie. This just picks the first one
            int currentMax = 0;
            int maxClass = 0;
            foreach (var kvp in majorities)
            {
                if (kvp.Value > currentMax)
                {
                    currentMax = kvp.Value;
                    maxClass = kvp.Key;
                }
            }

            return maxClass;
        }

        public void PrintTree(int indent = -1)
        {
            StringBuilder builder = new StringBuilder();

            // special case for beginning of tree
            if(Name != "Root")
            {
                for(int i = 0; i < indent; i++)
                {
                    builder.Append("| ");
                }
                builder.AppendFormat("{0} = {1} : ", Name, MatchValue);
            }

            if(!IsLeaf)
            {
                Console.WriteLine(builder.ToString());
                foreach(var kvp in _children)
                {
                    kvp.Key.PrintTree(indent + 1);
                }
            }
            else
            {
                builder.Append(Classification);
                Console.WriteLine(builder.ToString());
            }
        }
    }

    public static class DataRowExtensions
    {
        public static int RetrieveClassification(this DataRow row)
        {
            return row.RetrieveValueAsInt("class");
        }
    }
}
