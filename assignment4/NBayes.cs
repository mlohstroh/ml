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
            return string.Format("P({0}={1}|{2})={3:F4}", Attribute, AttributeValue, Classification, Probability);
        }
    }

    public class NBayes
    {
        private HashSet<string> _attributes = new HashSet<string>();
        private List<DataRow> _train = new List<DataRow>();
        private Dictionary<int, List<BayesContainer>> _probabilities = new Dictionary<int, List<BayesContainer>>();

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

            // make sure there are lists here
            _probabilities[0] = new List<BayesContainer>();
            _probabilities[1] = new List<BayesContainer>();
        }

        public void Train()
        {
            // lets figure out the split between the classes
            var splitByClassification = MathHelpers.CountByClassification(_train);

            // HACK! GLORIOUS HACK! I'm really just too lazy to think of a better way to store this at the moment.
            splitByClassification.TryGetValue(0, out countOfZero);
            splitByClassification.TryGetValue(1, out countOfOne);

            Console.Write("P(C={0})={1:F4} ", 0, GetProbabilityOfClassification(0));
            Console.Write("P(C={0})={1:F4} ", 1, GetProbabilityOfClassification(1));

            CalculateProbabilitiesForClass(0);
            CalculateProbabilitiesForClass(1);

            foreach (var c in _probabilities)
            {
                foreach (var l in c.Value)
                {
                    Console.Write(l);
                    Console.Write(" ");
                }
            }
        }

        private void CalculateProbabilitiesForClass(int classification)
        {
            List<DataRow> allOfClass = DataRow.GetDistByAttr(_train, "class")[classification];

            foreach (string attr in _attributes)
            {
                List<BayesContainer> container = GetProbabilitiesForAttribute(attr, classification, allOfClass);
                if (container != null)
                {
                    _probabilities[classification].AddRange(container);
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
                return (double)countOfZero / (double)_train.Count;
            }
            else
            {
                return (double)countOfOne / (double)_train.Count;
            }
        }

        public void Test(List<DataRow> test, string typeString = "Test")
        {
            int correct = 0;

            foreach (var row in test)
            {
                int actualClass = row.RetrieveClassification();

                // calculate for class zero
                double zero = ProbabiltyOfClassification(row, 0);
                // calculate for class one
                double one = ProbabiltyOfClassification(row, 1);

                if(zero >= one)
                {
                    // class is 0
                    if(actualClass == 0)
                    {
                        correct++;
                    }
                }
                else
                {
                    // class is 1
                    if(actualClass == 1)
                    {
                        correct++;
                    }
                }
            }

            double accuracy = (double)correct / (double)test.Count;

            Console.WriteLine("Accuracy on {0} set ({1} instances): {2:F2}%", typeString, test.Count, accuracy * 100);
        }

        private double ProbabiltyOfClassification(DataRow row, int classification)
        {
            double chanceOfClassification = GetProbabilityOfClassification(classification);
            List<BayesContainer> probability = _probabilities[classification];


            foreach(var attr in _attributes)
            {
                // evil extra loop... whatever. I don't care about speed here
                foreach(var container in probability)
                {
                    if (container.Attribute == attr)
                        chanceOfClassification *= container.Probability;
                }
            }

            return chanceOfClassification;
        }
    }
}
