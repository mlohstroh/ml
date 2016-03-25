using System.Collections.Generic;

namespace mlAssignment3
{
    public class State
    {
        public string Name { get; set; }
        public float Reward { get; set; }
        public List<MDPAction> Actions { get; set; }

        public State()
        {
            Actions = new List<MDPAction>();
        }
    }
}
