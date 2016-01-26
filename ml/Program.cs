﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace mlAssignment1
{
    class Program
    {
        static void Main(string[] args)
        {
            // testing entropy function
            //Console.WriteLine("Entropy: {0}", MathHelpers.Entropy(new double[] { 1.0/4.0, 3.0/4.0 }));

            ////// testing IG function
            //double pE = 1;
            //double rE = MathHelpers.Entropy(new double[] { 4.0 / 7.0, 3.0 / 7.0 });
            //double LE = MathHelpers.Entropy(new double[] { 2.0 / 3.0, 1.0 / 3.0 });

            //double lD = 3.0 / 10.0;
            //double rD = 7.0 / 10.0;
            //Console.WriteLine("IG: {0}", MathHelpers.InformationGained(pE, new double[] { LE, rE }, new double[] { lD, rD }));
            BooleanTreeNode root;
            if (args.Length != 2)
            {
                Console.WriteLine("Invalid input... Reading packaged test data...");

                string trainPath = @"data\train-win.dat";
                string testPath = @"data\test-win.dat";

                List<DataRow> trainData = ReadFile(trainPath);
                List<DataRow> testData = ReadFile(testPath);

                root = new BooleanTreeNode("Root", trainData);
                if (trainData.Count > 0)
                {
                    var hash = new HashSet<string>(trainData[0].Attributes);
                    // the reader counts the class as an attr. whoops
                    hash.Remove("class");
                    root.BuildTree(hash);
                }
                else
                {
                    Console.WriteLine("Uh oh...");
                }
            }

            //BuildTmpTree();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private static List<DataRow> ReadFile(string file)
        {
            List<DataRow> read = new List<DataRow>();

            using (StreamReader reader = new StreamReader(file))
            {
                // read the header

                string[] attrs = reader.ReadLine().Split(new char[] { '\t' });

                string line;
                while((line = reader.ReadLine()) != null)
                {
                    Dictionary<string, object> dict = new Dictionary<string, object>();
                    string[] data = line.Split(new char[] { '\t' });
                    for(int i = 0; i < attrs.Length; i++)
                    {
                        dict[attrs[i]] = int.Parse(data[i]);
                    }

                    read.Add(new DataRow(dict));

                }
            }

            return read;
        }

        private static void BuildTmpTree()
        {
            List<DataRow> tmpData = SpoofData(100, 3);

            BooleanTreeNode root = new BooleanTreeNode("Root", tmpData);
            if (tmpData.Count > 0)
            {
                root.BuildTree(new HashSet<string>(tmpData[0].Attributes));
            }

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
                stuff.Add("class", r.Next(100) % 2);
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
