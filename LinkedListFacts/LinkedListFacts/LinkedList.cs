using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedListFacts
{
    class LinkedList<T>
    {
        private int count;

        public LinkedList()
        {
            Head = null;
            count = 0;
        }

        public int Count { get => count; }

        public bool IsReadOnly { get; }

        public Node<T> Head { get; set; }

        public Node<T> Last { get => Head; set => Head = value; }
    }
}
