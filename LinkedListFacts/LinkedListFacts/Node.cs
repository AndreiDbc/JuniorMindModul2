using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedListFacts
{
    class Node<T>
    {
        public Node(T data)
        {
            this.Data = data;
            Previous = null;
            Next = null;
            IsContained = null;
        }

        public Node<T> Previous { get; set; }

        public Node<T> Next { get; set; }

        public T Data { get; }

        public LinkedList<T> IsContained { get; set; }

        public void CheckArgumentNullException()
        {
            if (this.Data != null)
            {
                return;
            }

            throw new ArgumentException("Node is NULL.");
        }
    }
}