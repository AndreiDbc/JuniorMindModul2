using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedListFacts
{
    class Node<T>
    {
        private T data;
        private Node<T> previous;
        private Node<T> next;
        public Node(T d)
        {
            data = d;
            previous = null;
            next = null;
        }

        public Node<T> Next { get { return next; } }


    }
}
