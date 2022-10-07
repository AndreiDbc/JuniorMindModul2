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
            this.children = new Node[0];
        }

        public Node[] Children { get => children; }

        public string Data { get => data; }

        public Node ChildAtDistance(int distance)
        {
            foreach (var child in this.Children)
            {
                if (GetDistance(child).Equals(distance))
                {
                    return child;
                }
            }

            return null;
        }

        public bool AddChild(Node toAdd)
        {
            Node child = this.ChildAtDistance(this.GetDistance(toAdd));
            if (child != null)
            {
                return false;
            }

            Array.Resize(ref children, children.Length + 1);
            children[^1] = toAdd;
            return true;
        }

        public int GetDistance(Node child)
        {
            int[,] matrix = new int[this.data.Length + 1, child.Data.Length + 1];
            FillEdges(matrix);

            for (int i = 1; i < this.data.Length + 1; i++)
            {
                for (int j = 1; j < child.Data.Length + 1; j++)
                {
                    matrix[i, j] = Math.Min(matrix[i - 1, j] + 1, Math.Min(matrix[i - 1, j - 1] + (this.data[i - 1].Equals(child.data[j - 1]) ? 0 : 1), matrix[i, j - 1] + 1));
                }
            }

            return matrix[this.data.Length, child.Data.Length];
        }

        public int GetDistance(string word)
        {
            return GetDistance(new Node(word));
        }

        private void FillEdges(int[,] matrix)
        {
            for (int i = 0; i < this.data.Length - 1; i++)
            {
                matrix[i, 0] = i;
            }

            for (int i = 0; i < matrix.Length / this.Data.Length - 1; i++)
            {
                matrix[0, i] = i;
            }
        }
    }
}
