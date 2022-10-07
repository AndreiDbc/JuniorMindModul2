using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xunit.Sdk;

namespace BKTree
{
    class Tree : IEnumerable<string>
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

        public IEnumerable<string> Find(string word)
        {
            if (root == null || root.GetDistance(word) > levenshteinDistance)
            {
                yield break;
            }

            yield return root.Data;
            Queue<Node> queue = new Queue<Node>();
            queue.Enqueue(root);
            while (queue.TryDequeue(out Node node))
            {
                foreach (var item in node.Children)
                {
                    if (item.GetDistance(word) <= levenshteinDistance)
                    {
                        queue.Enqueue(item);
                        yield return item.Data;
                    }
                }
            }
        }

        public IEnumerator<string> GetEnumerator()
        {
            yield return root.Data;
            Queue<Node> queue = new Queue<Node>();
            queue.Enqueue(root);
            while (queue.TryDequeue(out Node node))
            {
                foreach (var item in node.Children)
                {
                    yield return item.Data;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void AddToFreeNode(Node parent, Node toAdd)
        {
            if (parent.AddChild(toAdd))
            {
                return;
            }

            AddToFreeNode(parent.ChildAtDistance(parent.GetDistance(toAdd)), toAdd);
        }
    }
}
