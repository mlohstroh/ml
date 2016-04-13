using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mlAssignment1;

namespace assignment4
{
    class Program
    {
        static void Main(string[] args)
        {
            var train = DataRow.ReadFile("data\\train.dat");
            var test = DataRow.ReadFile("data\\test.dat");

            NBayes bayes = new NBayes(train);
            bayes.Train();

            Console.WriteLine();
            Console.WriteLine();

            bayes.Test(train, "Train");
            bayes.Test(test, "Test");

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
        }
    }
}
