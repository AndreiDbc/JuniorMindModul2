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
            toAdd.Next = previous.Next;
            previous.Next = toAdd;
            toAdd.Previous = previous;
            previous.Next = toAdd;
            Last = previous.Equals(Last) ? toAdd : Last;
            count++;
        }

        public void AddAfter(Node<T> previous, T toAdd)
        {
            Node<T> node = new Node<T>(toAdd);
            node.Next = previous.Next;
            previous.Next = node;
            node.Previous = previous;
            previous.Next = node;
            Last = previous.Equals(Last) ? node : Last;
            count++;
        }

        public void AddBefore(Node<T> next, Node<T> toAdd)
        {
            next.Previous.Next = toAdd;
            toAdd.Previous = next.Previous;
            toAdd.Next = next.Next;
            next.Previous = toAdd;
            head = next.Equals(head.Next) ? toAdd : head;
            count++;
        }

        public void AddBefore(Node<T> next, T toAdd)
        {
            Node<T> node = new Node<T>(toAdd);
            next.Previous.Next = node;
            node.Previous = next.Previous;
            node.Next = next.Next;
            next.Previous = node;
            head = next.Equals(head.Next) ? node : head;
            count++;
        }

        public void AddFirst(Node<T> toAdd)
        {
            AddBefore(head.Next, toAdd);
        }

        public void AddFirst(T toAdd)
        {
            AddBefore(head.Next, toAdd);
        }

        public void AddLast(Node<T> toAdd)
        {
            AddAfter(Last, toAdd);
        }

        public void AddLast(T toAdd)
        {
            AddAfter(Last, toAdd);
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
            Node<T> node = Find(toDelete);
            if (!Contains(toDelete))
            {
                return false;
            }

            var temp = node.Next;
            temp.Previous = node.Previous;
            node.Next = temp;
            node.Previous.Next = temp;
            Last = node == Last ? Last.Previous : Last;
            count--;
            return true;
        }

        public bool Remove(Node<T> toDelete)
        {
            if (!Contains(toDelete.Data))
            {
                return false;
            }

            var temp = toDelete.Next;
            temp.Previous = toDelete.Previous;
            toDelete.Next = temp;
            toDelete.Previous.Next = temp;
            Last = toDelete == Last ? Last.Previous : Last;
            count--;
            return true;
        }

        public void RemoveFirst()
        {
            Remove(head.Next);
        }

        public void RemoveLast()
        {
            Remove(Last);
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

        private Node<T> Find(T data)
        {
            for (Node<T> i = head.Next; i != head; i = i.Next)
            {
                if (i.Data.Equals(data))
                {
                    return i;
                }
            }

            return null;
        }
    }
}
