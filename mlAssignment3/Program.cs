using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace mlAssignment3
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length != 4)
            {
                Console.WriteLine("Incorrect arguments...");
            }
            int numActions = 2;
            string file = "data\\test.in";
            float gamma = 0.9f;

            try
            {
                numActions = int.Parse(args[1]);

                file = args[2];
                gamma = float.Parse(args[3]);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception reading arguments...");
                Console.WriteLine("Please enter exactly 4 arguments");
            }

            MDP mdp = new MDP()
            {
                Gamma = gamma
            };

            using (StreamReader reader = new StreamReader(file))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    
                    string[] data = line.Split(new char[] { '\t', ' ' });

                    if (data.Length > 2)
                    {
                        string name = data[0];
                        float reward = float.Parse(data[1]);
                        State s = new State()
                        {
                            Name = name,
                            Reward = reward
                        };

                        List<MDPAction> actions = new List<MDPAction>();

                        // define actions for each states
                        for(int i = 0; i < numActions; i++)
                        {
                            actions.Add(new MDPAction()
                            {
                                Name = string.Format("a{0}", i + 1)
                            });
                        }

                        // begin the transitions
                        for(int i = 2; i < data.Length; i += 3)
                        {
                            string actionName = data[i].Substring(1);
                            string stateKey = data[i + 1];
                            string tmp = data[i + 2];
                            float probability = float.Parse(tmp.Substring(0, tmp.Length - 1));

                            var action = actions.Where(x => x.Name == actionName).First();

                            action.AddTransition(stateKey, probability);
                        }

                        s.Actions = actions;
                        mdp.AddState(s);
                    }
                }
            }


            mdp.SimulateAll();

            Console.ReadKey(true);
        }
    }
}
