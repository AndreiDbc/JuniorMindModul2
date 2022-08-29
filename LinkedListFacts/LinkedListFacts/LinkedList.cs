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
            head.Next = head;
            head.Previous = head;
            Last = head;
            count = 0;
        }

        public int Count { get => count; }

        public bool IsReadOnly { get; }

        public Node<T> Last { get; set; }

        public void Add(T toAdd)
        {
            Node<T> node = new Node<T>(toAdd);
            Last.Next = node;
            head.Previous = node;
            node.Previous = Last;
            Last = node;
            node.Next = head;
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
            head.Next = head;
            head.Previous = head;
            Last = head;
            count = 0;
        }

        public bool Remove(T toDelete)
        {
            for (Node<T> i = head.Next; i != head; i = i.Next)
            {
                if (i.Data.Equals(toDelete))
                {
                    i.Previous = GetNextNode(toDelete);
                    i.Next.Previous = GetPreviousNode(toDelete);
                    count--;
                    return true;
                }
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
            for (Node<T> i = head.Next; i != head; i = i.Next)
            {
                if (i.Equals(data))
                {
                    return i.Next;
                }
            }

            return head;
        }
    }
}
