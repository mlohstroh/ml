using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mlAssignment3
{
    public class Policy
    {
        public State State { get; set; }
        public MDPAction Action { get; set; }
        public float J { get; set; }        

        public override string ToString()
        {
            return string.Format("({0} {1} {2:F3})", State.Name, Action.Name, J);
        }
    }
}
