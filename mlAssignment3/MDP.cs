using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mlAssignment3
{
    public class MDP
    {
        /*
        * Markov Decision Process Model
        * Includes 
        *  - a set of states
        *  - a set of actions
        *  - a set of rewards (one for each state)
        *  - a transition probability function
        */

        // number of times we have to simulate the MDP
        public const int Iterations = 20;

        public float Gamma { get; set; }
        // hard coded for now
        public List<Policy>[] _policies = new List<Policy>[Iterations];

        private List<State> _states = new List<State>();
        private int _currentIteration = 0;

        public MDP()
        {
            // init the list
            for(int i = 0; i < Iterations; i++)
            {
                _policies[i] = new List<Policy>();
            }
        }

        public void AddState(State s)
        {
            _states.Add(s);
        }

        public void SimulateAll()
        {
            for(int i = 0; i < Iterations; i++)
            {
                SimulateStep();
            }
        }

        public void SimulateStep()
        {
            // Call current state
            // receive reward r
            // choose action
            // choose which state you move based on which action
            // all future rewards are discounted by delta

            List<Policy> policies = _policies[_currentIteration];

            for(int i = 0; i < _states.Count; i++)
            {
                State current = _states[i];

                var tuple = ActionSum(current);

                // just set the reward
                policies.Add(new Policy()
                {
					J = tuple.Item2,
                    State = current,
                    Action = tuple.Item1
                });
            }

            Console.Write("After iteration {0}: ", _currentIteration + 1);

            foreach (var p in policies)
            {
                Console.Write(p);
                Console.Write(" ");
            }

            Console.WriteLine();
            _currentIteration++;
        }

        private Tuple<MDPAction, float> ActionSum(State current)
        {
			Dictionary<MDPAction, float> tmp = new Dictionary<MDPAction, float> ();

            for (int x = 0; x < current.Actions.Count; x++)
            {
                MDPAction a = current.Actions[x];
                float sum = 0;
                foreach(var kvp in a.Chances)
                {
					sum += GetJAtPreviousIteration(kvp.Key) * kvp.Value;
                }
                
				tmp.Add (a, sum);
            }

			var max = tmp.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;

			// current reward + gamma * action_sum
			return new Tuple<MDPAction, float>(max, current.Reward + Gamma * tmp[max]);
        }

        private float GetJAtPreviousIteration(string stateLookup)
        {
            State state = null;
            for(int i = 0; i < _states.Count; i++)
            {
                if (_states[i].Name == stateLookup)
                {
                    state = _states[i];
                    break;
                }
            }

            List<Policy> previousPolicies = null;
            if (_currentIteration > 0)
            {
                // then set the previous policy
                previousPolicies = _policies[_currentIteration - 1];

                for(int i = 0; i < previousPolicies.Count; i++)
                {
                    if(previousPolicies[i].State.Name == state.Name)
                    {
                        return previousPolicies[i].J;
                    }
                }
            }

            return state.Reward;
        }
    }
}
