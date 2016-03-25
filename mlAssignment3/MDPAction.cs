using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mlAssignment3
{
    public class MDPAction
    {
        public string Name { get; set; }
        public Dictionary<string, float> Chances = new Dictionary<string, float>();

        public void AddTransition(string transitionTo, float chance)
        {
            Chances.Add(transitionTo, chance);
        }
    }
}
