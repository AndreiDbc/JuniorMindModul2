using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedListFacts
{
    class Node<T>
    {
        private readonly T data;
        private readonly Node<T> previous;
        private readonly Node<T> next;

        public Node(T d)
        {
            data = d;
            previous = null;
            next = null;
        }

        public Node<T> Previous { get => previous; }

        public Node<T> Next { get => next; }

        public T Data { get => data; }
    }
}
