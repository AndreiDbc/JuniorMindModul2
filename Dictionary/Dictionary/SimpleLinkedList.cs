using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionary
{
    class SimpleLinkedList<T>
    {
        private Node<T> head;
        private int count;

        public SimpleLinkedList()
        {
            head = new Node<T>(default(T));
            head.Next = null;
            count = 0;
        }

        public int Count { get => count; }

        public Node<T> Last
        {
            get
            {
                int temp = count;
                for (Node<T> i = head.Next; i != null; i = i.Next)
                {
                    if (temp == 0)
                    {
                        return i;
                    }

                    temp--;
                }

                return head;
            }
        }

        public bool IsReadOnly { get; }

        public void Add(T data)
        {
            Node<T> node = new (data);
            head = node;
            node.Next = count == 0 ? null : head;
            count++;
        }

        public void Clear()
        {
            head = new Node<T>(default(T));
            head.Next = null;
            count = 0;
        }
    }
}
