using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKTree
{
    class Tree
    {
        private readonly int levenshteinDistance;
        private Node root;

        public Tree(int levenshteinDistance)
        {
            this.levenshteinDistance = levenshteinDistance;
        }

        public void Add(string data)
        {
            if (root == null)
            {
                root = new Node(data);
                return;
            }

            AddToFreeNode(root, new Node(data));
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
