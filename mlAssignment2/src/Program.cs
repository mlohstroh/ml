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
            string trainFile = "data\\train2-win.dat";
            string testFile = "data\\test2-win.dat";
            float learningRate = 5f;
            int numIterations = 4000;

            if (args.Length >= 4)
            {
                // It goes 
                // trainFile testFile learningRate and numberofIterations
                
                trainFile = args[0];
                testFile = args[1];

                try
                {
                    learningRate = float.Parse(args[2]);
                }
                catch(Exception)
                {
                    Console.WriteLine("Unable to parse learning rate. Correct example: 0.4");
                }

                try
                {
                    numIterations = int.Parse(args[3]);
                }
                catch (Exception)
                {
                    Console.WriteLine("Unable to parse number of iterations. Correct example: 400");
                }
            }
            else
            {
                Console.WriteLine("You provided an incorrect number of inputs... Defauting to the packaged parameters");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey(true);
            }

            List<DataRow> train = DataRow.ReadFile(trainFile);
            List<DataRow> test = DataRow.ReadFile(testFile);

            Perceptron p = new Perceptron(train)
            {
                DefaultEdgeWeight = 0,
                LearningRate = learningRate,
                Threshold = 0.5f,
                MaxIterations = numIterations
            };

            p.Train(train);

            float trainAccuracy = p.Classify(train);
            float testAccuracy = p.Classify(test);
            Console.WriteLine();
            Console.WriteLine("Accuracy on training set ({0} instances): {1}%", train.Count, trainAccuracy);
            Console.WriteLine("Accuracy on test set ({0} instances): {1}%", test.Count, testAccuracy);

            Console.ReadKey(true);
        }
    }
}
