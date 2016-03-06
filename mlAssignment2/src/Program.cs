using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mlAssignment1;

namespace mlAssignment2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            List<DataRow> train = DataRow.ReadFile("data\\train2-win.dat");
            List<DataRow> test = DataRow.ReadFile("data\\test2-win.dat");

            Perceptron p = new Perceptron(train)
            {
                DefaultEdgeWeight = 0,
                LearningRate = 0.9f,
                Threshold = 0.5f,
                MaxIterations = 800
            };

            p.Train(train);

            float trainAccuracy = p.Classify(train);
            float testAccuracy = p.Classify(test);
            Console.WriteLine("Accuracy on training set ({0} instances): {1}%", train.Count, trainAccuracy);
            Console.WriteLine("Accuracy on test set ({0} instances): {1}%", test.Count, testAccuracy);

            Console.ReadKey(true);
        }
    }
}
