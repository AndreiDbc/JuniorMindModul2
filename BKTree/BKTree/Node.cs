using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKTree
{
    class Node
    {
        private readonly string data;
        private Node[] children;

        public Node(string data)
        {
            this.data = data;
            this.children = null;
        }

        public Node[] Next { get => children; }

        public string Data { get => data; }

        public Node ChildAtDistance(int distance)
        {
            foreach (var child in this.Next)
            {
                if (GetDistance(child).Equals(distance))
                {
                    return child;
                }
            }

            return null;
        }

        public bool AddToParent(Node toAdd)
        {
            if (this.Next == null)
            {
                this.AddChild(toAdd);
                return true;
            }

            Node child = this.ChildAtDistance(this.GetDistance(toAdd));
            if (child != null)
            {
                return false;
            }

            this.AddChild(toAdd);
            return true;
        }

        public bool AddToChild(Node toAdd)
        {
            foreach (var node in this.Next)
            {
                if (node.GetDistance(toAdd) == null)
                {
                    node.AddChild(toAdd);
                    return true;
                }
            }

            return false;
        }

        public int GetDistance(Node child)
        {
            int parentLength = this.data.Length;
            int childLength = child.data.Length;
            int[,] matrix = new int[parentLength + 1, childLength + 1];
            FillEdges(matrix, childLength);

            for (int i = 1; i < parentLength + 1; i++)
            {
                for (int j = 1; j < childLength + 1; j++)
                {
                    matrix[i, j] = Math.Min(matrix[i - 1, j] + 1, Math.Min(matrix[i - 1, j - 1] + (this.data[i - 1].Equals(child.data[j - 1]) ? 0 : 1), matrix[i, j - 1] + 1));
                }
            }

            return matrix[parentLength, childLength];
        }

        private void AddChild(Node toAdd)
        {
            children ??= new Node[0];

            Array.Resize(ref children, children.Length + 1);
            children[children.Length - 1] = toAdd;
        }

        private void FillEdges(int[,] matrix, int childLength)
        {
            for (int i = 0; i < this.data.Length + 1; i++)
            {
                matrix[i, 0] = i;
            }

            for (int i = 0; i < childLength + 1; i++)
            {
                matrix[0, i] = i;
            }
        }
    }
}
