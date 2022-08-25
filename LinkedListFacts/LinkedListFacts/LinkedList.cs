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
            count = 0;
        }

        public int Count { get => count; }

        public bool IsReadOnly { get; }

        public Node<T> Head { get; set; }

        public Node<T> Last { get; set; }

        public void Add(T toAdd)
        {
            if (count == 0)
            {
                InsertFront(toAdd);
            }
            else
            {
                InsertLast(toAdd);
            }

            count++;
        }

        public bool Contains(T data)
        {
            Node<T> node = Head.Next;
            for (int i = 0; i < Count; i++)
            {
                if (node.Data.Equals(data))
                {
                    return true;
                }

                node = node.Next;
            }

            return false;
        }

        public void CopyTo(T[] array, int location)
        {
            Node<T> node = Head.Next;
            for (int i = 0; i < Count; i++)
            {
                array[location] = node.Data;
                location++;
                node = node.Next;
            }
        }

        private void InsertFront(T toAdd)
        {
            Node<T> node = new Node<T>(toAdd);
            Head.Next = node;
            Head.Previous = node;
            Last = node;
            node.Previous = Head;
            node.Next = Head;
        }

        private void InsertLast(T toAdd)
        {
            Node<T> node = new Node<T>(toAdd);
            Last.Next = node;
            Head.Previous = node;
            node.Previous = Last;
            Last = node;
            node.Next = Head;
        }
    }
}
