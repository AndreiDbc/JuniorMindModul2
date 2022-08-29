using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedListFacts
{
    class LinkedList<T> : ICollection<T>
    {
        private readonly Node<T> head;
        private int count;

        public LinkedList()
        {
            head = new Node<T>(default(T));
            Last = head;
            count = 0;
        }

        public int Count { get => count; }

        public bool IsReadOnly { get; }

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
            foreach (var value in this)
            {
                if (value.Equals(data))
                {
                    return true;
                }
            }

            return false;
        }

        public void CopyTo(T[] array, int location)
        {
            Node<T> node = head.Next;
            for (int i = 0; i < Count; i++)
            {
                array[location] = node.Data;
                location++;
                node = node.Next;
            }
        }

        public void Clear()
        {
            head.Next = null;
            head.Previous = null;
            Last = head;
            count = 0;
        }

        public bool Remove(T toDelete)
        {
            Node<T> node = head;
            for (int i = 0; i < Count + 1; i++)
            {
                if (i == GetPreviousLocation(toDelete))
                {
                    node.Next = GetNextNode(toDelete);
                    Node<T> temp = node;
                    node = node.Next;
                    node.Previous = temp;
                    Last = i == count - 1 ? node.Previous : Last;
                    count--;
                    return true;
                }

                node = node.Next;
            }

            return false;
        }

        public IEnumerator<T> GetEnumerator()
        {
            Node<T> node = head;
            for (int i = 0; i < Count; i++)
            {
                node = node.Next;
                yield return node.Data;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private Node<T> GetPreviousNode(T data)
        {
            for (Node<T> i = head.Next; i != head; i = i.Next)
            {
                if (i.Data.Equals(data))
                {
                    return i.Previous;
                }
            }

            return head;
        }

        private Node<T> GetNextNode(T data)
        {
            Node<T> node = head.Next;
            for (int i = 0; i < Count; i++)
            {
                if (node.Data.Equals(data))
                {
                    return node.Next;
                }

                node = node.Next;
            }

            return head;
        }

        private void InsertFront(T toAdd)
        {
            Node<T> node = new Node<T>(toAdd);
            head.Next = node;
            head.Previous = node;
            Last = node;
            node.Previous = head;
            node.Next = head;
        }

        private void InsertLast(T toAdd)
        {
            Node<T> node = new Node<T>(toAdd);
            Last.Next = node;
            head.Previous = node;
            node.Previous = Last;
            Last = node;
            node.Next = head;
        }
    }
}
