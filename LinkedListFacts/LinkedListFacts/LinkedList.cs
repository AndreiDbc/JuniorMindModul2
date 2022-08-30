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
        private Node<T> head;
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

        public void AddAfter(Node<T> previous, Node<T> toAdd)
        {
            for (Node<T> i = head.Next; i != head; i = i.Next)
            {
                if (i.Equals(previous))
                {
                    toAdd.Next = i.Next;
                    i.Next = toAdd;
                    toAdd.Previous = i;
                    i.Next = toAdd;
                    Last = i.Equals(Last) ? toAdd : Last;
                    count++;
                    return;
                }
            }
        }

        public void AddAfter(Node<T> previous, T toAdd)
        {
            for (Node<T> i = head.Next; i != head; i = i.Next)
            {
                if (i.Equals(previous))
                {
                    Node<T> node = new Node<T>(toAdd);
                    node.Next = i.Next;
                    i.Next = node;
                    node.Previous = i;
                    i.Next = node;
                    Last = i.Equals(Last) ? node : Last;
                    count++;
                    return;
                }
            }
        }

        public void AddBefore(Node<T> next, Node<T> toAdd)
        {
            for (Node<T> i = head.Next; i != head; i = i.Next)
            {
                if (i.Equals(next))
                {
                    i.Previous.Next = toAdd;
                    toAdd.Previous = i.Previous;
                    toAdd.Next = i.Next;
                    i.Previous = toAdd;
                    head = i.Equals(head.Next) ? toAdd : head;
                    count++;
                    return;
                }
            }
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
                    var temp = i.Next;
                    temp.Previous = i.Previous;
                    i.Next = temp;
                    i.Previous.Next = temp;
                    Last = i == Last ? Last.Previous : Last;
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
    }
}
