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
            while (queue.TryDequeue(out Node node))
            {
                foreach (var item in GetNodes(queue, node, word))
                {
                    yield return item.Data;
                }
            }
        }

        private Node[] GetNodes(Queue<Node> queue, Node node, string word)
        {
            Node[] result = new Node[0];
            for (int i = 0; i < node.Next.Length; i++)
            {
                if (node.Next[i].GetDistance(word) <= levenshteinDistance)
                {
                    queue.Enqueue(node.Next[i]);
                    Array.Resize(ref result, result.Length + 1);
                    result[result.Length - 1] = node.Next[i];
                }
            }

            return result;
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
