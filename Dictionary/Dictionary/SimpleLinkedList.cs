using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionary
{
    class SimpleLinkedList<T>
    {
        private readonly Node<T> head;
        private readonly int count;

        public SimpleLinkedList()
        {
            head = new Node<T>(default(T));
            head.Next = head;
            count = 0;
        }

        public int Count { get => count; }
    }
}
