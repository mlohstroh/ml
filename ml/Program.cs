using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mlAssignment1
{
    class Program
    {
        static void Main(string[] args)
        {
            // testing entropy function
            Console.WriteLine("Entropy: {0}", MathHelpers.Entropy(new double[] { 3.0/5.0, 2.0/5.0 }));

            // testing IG function
            double pE = 0.99;
            double LE = 0;
            double rE = 0.58;

            double lD = 4.0 / 11.0;
            double rD = 7.0 / 11.0;
            Console.WriteLine("IG: {0}", MathHelpers.InformationGained(pE, new double[] { LE, rE }, new double[] { lD, rD }));

            BuildTmpTree();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private static void BuildTmpTree()
        {
            List<DataRow> tmpData = SpoofData(100, 3);

            BinaryTreeBuilder builder = new BinaryTreeBuilder(tmpData);
        }

        private static List<DataRow> SpoofData(int count, int attributeCount)
        {
            List<DataRow> data = new List<DataRow>(count);

            List<string> attributes = new List<string>(attributeCount);

            Random r = new Random();

            for(int i = 0; i < attributeCount; i++)
            {
                attributes.Add(TestTmpString(r));
            }

            for(int i = 0; i < count; i++)
            {
                Dictionary<string, object> stuff = new Dictionary<string, object>();
                for(int x = 0; x < attributeCount; x++)
                {
                    stuff.Add(attributes[x], r.Next(100) % 2);
                }
                // manually add the classification
                stuff.Add("classification", r.Next(100) % 2);
                data.Add(new DataRow(stuff));
            }

            return data;
        }

        private static string TestTmpString(Random r)
        {
            // total test function
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 10)
              .Select(s => s[r.Next(s.Length)]).ToArray());
        }
    }
}
