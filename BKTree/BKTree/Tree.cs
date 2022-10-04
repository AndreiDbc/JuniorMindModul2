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
            if (parent.AddToParent(toAdd))
            {
                return;
            }

            if (parent.AddToChild(toAdd))
            {
                return;
            }

            AddToFreeNode(parent.ChildAtDistance(parent.GetDistance(toAdd)), toAdd);
        }
    }
}
