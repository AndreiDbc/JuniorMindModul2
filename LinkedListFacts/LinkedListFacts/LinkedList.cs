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
            Head = new Node<T>(default(T));
            count = 1;
        }

        public int Count { get => count; }

        public bool IsReadOnly { get; }

        public Node<T> Head { get; set; }

        public Node<T> Last { get; set; }

        public void Add(Node<T> toAdd)
        {
            if (count == 1)
            {
                InsertFront(toAdd);
            }
            else
            {
                InsertLast(toAdd);
            }

            count++;
        }

        private void InsertFront(Node<T> toAdd)
        {
            Head.Next = toAdd;
            Head.Previous = toAdd;
            Last = toAdd;
            toAdd.Previous = Head;
            toAdd.Next = Head;
        }

        private void InsertLast(Node<T> toAdd)
        {
            Last.Next = toAdd;
            Head.Previous = toAdd;
            toAdd.Previous = Last;
            Last = toAdd;
            toAdd.Next = Head;
        }
    }
}
