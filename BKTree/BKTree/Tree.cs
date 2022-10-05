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

        public List<string> ReturnWords(string word)
        {
            if (root == null || root.GetDistance(word) > levenshteinDistance)
            {
                return null;
            }

            List<string> words = new List<string>();
            words.Add(root.Data);
            GetWords(root, word, words);

            return words;
        }

        private void GetWords(Node parent, string word, List<string> words)
        {
            if (parent.Next == null)
            {
                return;
            }

            foreach (var node in parent.Next)
            {
                if (node.GetDistance(word) <= levenshteinDistance)
                {
                    words.Add(node.Data);
                }

                GetWords(node, word, words);
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
