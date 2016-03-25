using System;
using System.Collections.Generic;
using mlAssignment1;

namespace mlAssignment2
{
    public class Perceptron
    {
        public List<Edge> _edges = new List<Edge>();

        public float LearningRate { get; set; }
        public int MaxIterations { get; set; }
        public float DefaultEdgeWeight { get; set; }
        public float Threshold { get; set; }

        public Perceptron(List<DataRow> populateData)
        {
            if(populateData.Count > 0)
            {
                // lets create our edges!
                CreateEdges(new HashSet<string>(populateData[0].Attributes));
            }
        }

        private void CreateEdges(HashSet<string> attributes)
        {
            // always remove the class var
            attributes.Remove("class");

            Console.WriteLine("Creating {0} edges", attributes.Count);

            foreach(string attr in attributes)
            {
                Edge e = new Edge()
                {
                    Attribute = attr,
                    Weight = DefaultEdgeWeight
                };
                _edges.Add(e);
            }
        }

        public void Train(List<DataRow> data)
        {
            Console.WriteLine("Training over {0} data points", data.Count);

            // TODO: loop until we are done or error is minimal
            for(int i = 0; i < MaxIterations; i++)
            {
                DataRow current = null;
                if (i >= data.Count)
					current = data[data.Count % i];
                else
                    current = data[i];

                double unsigmoided;
                double weightedSum = GetWeightedSum(current, out unsigmoided);
                int computed = ClassifyWithThreshold(weightedSum);
                int actual = current.RetrieveClassification();

                Console.Write("After iteration {0}: ", i);
                for (int x = 0; x < _edges.Count; x++)
                {
                    Edge currentEdge = _edges[x];
                    int attrValue = current.RetrieveValueAsInt(currentEdge.Attribute);
					float newWeight = currentEdge.Weight + LearningRate * (float)(actual - weightedSum) * (float)attrValue * (float)MathHelpers.DerivativeSigmoid(unsigmoided);
                    currentEdge.Weight = newWeight;
                    Console.Write(currentEdge);
                    Console.Write(" ");
                }
                Console.Write("output = {0}", weightedSum);
                Console.WriteLine();
            }
        }
    
        // Returns the accuracy
        public float Classify(List<DataRow> data)
        {
            int correctClassifications = 0;

            for(int i = 0; i < data.Count; i++)
            {
                int actual = data[i].RetrieveClassification();
                double tmp;
                int computed = ClassifyWithThreshold(GetWeightedSum(data[i], out tmp));
                if (computed == actual)
                    correctClassifications++;
            }

            return ((float)correctClassifications / (float)data.Count) * 100.0f;
        }

        private int ClassifyWithThreshold(double weightedSum)
        {
            int computed = 1;

            if (weightedSum > Threshold)
            {
                computed = 1;
            }
            else
            {
                computed = 0;
            }

            return computed;
        }

        private double GetWeightedSum(DataRow row, out double unactivated)
        {
            double weightedSum = 0;

            for (int x = 0; x < _edges.Count; x++)
            {
                Edge currentEdge = _edges[x];
                weightedSum += currentEdge.Weight * row.RetrieveValueAsInt(currentEdge.Attribute);
            }

            unactivated = weightedSum;

            // now that we have the weighted sum, lets sigmoid it
			return 1.0 / (1.0 + Math.Pow(Math.E, -weightedSum));
        }
    }
}
