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
        private List<string> words;

        public Tree(int levenshteinDistance)
        {
            this.levenshteinDistance = levenshteinDistance;
            words = new List<string>();
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

            words = GetWords(root, word);
            words.Add(root.Data);

            return words;
        }

        private List<string> GetWords(Node parent, string word)
        {
            if (parent.Next == null)
            {
                return words;
            }

            foreach (var node in parent.Next)
            {
                if (node.GetDistance(word) <= levenshteinDistance)
                {
                    words.Add(node.Data);
                }

                GetWords(node, word);
            }

            return words;
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
