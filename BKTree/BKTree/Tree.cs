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
            return first.Concat(GetWords(word));
        }

        private IEnumerable<string> GetWords(string word)
        {
            Queue<Node> queue = new Queue<Node>();
            queue.Enqueue(root);
            while (queue.Count != 0)
            {
                int count = queue.Count;
                while (count > 0)
                {
                    Node node = queue.Peek();
                    queue.Dequeue();
                    foreach (var item in GetNodes(queue, node, word))
                    {
                        yield return item;
                    }

                    count--;
                }
            }
        }

        private IEnumerable<string> GetNodes(Queue<Node> queue, Node node, string word)
        {
            for (int i = 0; i < node.Next.Length; i++)
            {
                if (node.Next[i].GetDistance(word) <= levenshteinDistance)
                {
                    queue.Enqueue(node.Next[i]);
                    yield return node.Next[i].Data;
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
