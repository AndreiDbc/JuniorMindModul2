using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xunit.Sdk;

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

        public IEnumerable<string> ReturnWords(string word)
        {
            if (root == null || root.GetDistance(word) > levenshteinDistance)
            {
                return null;
            }

            IEnumerable<string> first = new[] { root.Data };
            return first.Concat(GetWords(root, word));
        }

        private IEnumerable<string> GetWords(Node parent, string word)
        {
            if (parent.Next == null)
            {
                yield break;
            }

            foreach (var node in parent.Next)
            {
                if (node.GetDistance(word) <= levenshteinDistance)
                {
                    yield return node.Data;
                }

                foreach (var child in GetWords(node, word))
                {
                    yield return child;
                }
            }
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
