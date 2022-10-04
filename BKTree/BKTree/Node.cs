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
    }
}
