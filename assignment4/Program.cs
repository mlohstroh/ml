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
            if(args.Length != 2)
            {
                Console.WriteLine("Incorrect arguments... Please enter two arguments, first is train, second is test");
                return;
            }

            List<DataRow> train = null;
            List<DataRow> test = null;

            try
            {
                train = DataRow.ReadFile(args[0]);
                test = DataRow.ReadFile(args[1]);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error reading arguments! Dumping excemption: {0}", ex);
                return;
            }

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
