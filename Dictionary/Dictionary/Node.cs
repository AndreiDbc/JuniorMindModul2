using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionary
{
    class Node<T>
    {
        public Node(T data)
        {
            this.Data = data;
            Next = null;
        }

        public Node<T> Next { get; set; }

        public T Data { get; }
    }
}
