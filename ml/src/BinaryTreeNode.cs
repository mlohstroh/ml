using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mlAssignment1
{
    public class BooleanTreeNode
    {
        public string Name { get; private set; }

        /// <summary>
        /// The return result of the classification. This is only applicable for leaf nodes
        /// </summary>
        public int Classification { get; set; }
        private Dictionary<BooleanTreeNode, Func<DataRow, bool>> _children = new Dictionary<BooleanTreeNode, Func<DataRow, bool>>();

        public bool IsLeaf
        {
            get { return _children.Count == 0; }
        }

        public BooleanTreeNode(string name)
        {
            Name = name;
        }

        public void AddChild(BooleanTreeNode node, Func<DataRow, bool> resultFunc)
        {
            _children.Add(node, resultFunc);
        }

        public int Classify(DataRow row)
        {
            if (IsLeaf)
            {
                return Classification;
            }

            foreach(var kvp in _children)
            {
                if(kvp.Value(row))
                {
                    return kvp.Key.Classify(row);
                }
            }
            return 0;
        }
    }
}
