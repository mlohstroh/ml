using System;
using System.Collections.Generic;
using System.Linq;
using mlAssignment1;

namespace assignment4
{
    public class BayesContainer
    {
        public int Classification { get; set; }
        public double Probability { get; set; }

        public int AttributeValue { get; set; }
        public string Attribute { get; set; }

        public override string ToString()
        {
            return string.Format("P({0}={1}|{2})={3}", Attribute, AttributeValue, Classification, Probability);
        }
    }

    public class NBayes
    {
        private HashSet<string> _attributes = new HashSet<string>();
        private List<DataRow> _train = new List<DataRow>();
        private List<BayesContainer> _probabilities = new List<BayesContainer>();

        // Hack: I need these two variables for the binary split
        int countOfZero = 0;
        int countOfOne = 0;

        public NBayes(List<DataRow> train)
        {
            _train = train;
            if(_train.Count > 0)
            {
                _attributes = new HashSet<string>(_train[0].Attributes);
                _attributes.Remove("class");
            }
        }

        public void Train()
        {
            // lets figure out the split between the classes
            var splitByClassification = MathHelpers.CountByClassification(_train);

            // HACK! GLORIOUS HACK! I'm really just too lazy to think of a better way to store this at the moment.
            splitByClassification.TryGetValue(0, out countOfZero);
            splitByClassification.TryGetValue(1, out countOfOne);

            Console.Write("P(C={0})={1}", 0, GetProbabilityOfClassification(0));
            Console.Write("P(C={0})={1}", 1, GetProbabilityOfClassification(1));

            CalculateProbabilitiesForClass(0);
            CalculateProbabilitiesForClass(1);

            foreach(var c in _probabilities)
            {
                Console.WriteLine(c);
            }

            // print them
        }

        public void Test(List<DataRow> test)
        {

        }

        private void CalculateProbabilitiesForClass(int classification)
        {
            List<DataRow> allOfClass = DataRow.GetDistByAttr(_train, "class")[classification];

            foreach (string attr in _attributes)
            {
                List<BayesContainer> container = GetProbabilitiesForAttribute(attr, classification, allOfClass);
                if (container != null)
                {
                    _probabilities.AddRange(container);
                }
            }
        }

        private List<BayesContainer> GetProbabilitiesForAttribute(string attr, int classification, List<DataRow> subset)
        {
            var split = DataRow.GetDistByAttr(subset, attr);

            List<BayesContainer> containers = new List<BayesContainer>();

            foreach(var kvp in split)
            {
                double probability = (double)kvp.Value.Count / (double)subset.Count;
                BayesContainer container = new BayesContainer()
                {
                    Attribute = attr,
                    Classification = classification,
                    Probability = probability,
                    AttributeValue = kvp.Key
                };

                containers.Add(container);
            }

            return containers;
        }

        private double GetProbabilityOfClassification(int @class)
        {
            if(@class == 0)
            {
                return countOfZero;
            }
            else
            {
                return countOfOne;
            }
        }
    }
}
