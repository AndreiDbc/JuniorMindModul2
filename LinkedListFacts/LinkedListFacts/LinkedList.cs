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
        private readonly Node<T> head;
        private int count;

        public LinkedList()
        {
            head = null;
            count = 0;
        }

        public int Count { get => count; }

        public bool IsReadOnly { get; }

        public Node<T> First { get => head; }

        public Node<T> Last
        {
            get
            {
                var temp = head;
                for (int i = 0; i < Count; i++)
                {
                    temp = temp.Next;
                }

                return temp;
            }
        }
    }
}
