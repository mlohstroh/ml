using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mlAssignment2
{
    public class Edge
    {
        public float Weight { get; set; }
        public string Attribute { get; set; }

        public override string ToString()
        {
            return string.Format("w({0}) = {1}", Attribute, Weight);
        }
    }
}
