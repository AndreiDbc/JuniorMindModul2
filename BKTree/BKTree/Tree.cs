using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKTree
{
    class Tree
    {
        private Node root;

        public Tree()
        {
        }

        public void Add(Node toAdd)
        {
            if (root == null)
            {
                root = toAdd;
                return;
            }

            AddToFreeNode(root, toAdd);
        }

        private void AddToFreeNode(Node parent, Node toAdd)
        {
            if (parent.Next == null)
            {
                parent.AddChild(toAdd);
                return;
            }

            Node child = parent.ChildAtDistance(parent.GetDistance(toAdd));
            if (child == null)
            {
                parent.AddChild(toAdd);
                return;
            }

            foreach (var node in parent.Next)
            {
                if (node.GetDistance(toAdd) == null)
                {
                    node.AddChild(toAdd);
                    return;
                }
            }

            AddToFreeNode(child, toAdd);
        }
    }
}
