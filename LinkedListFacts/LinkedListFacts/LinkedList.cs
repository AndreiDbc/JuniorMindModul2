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
            AddAfter(Last, toAdd);
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
            AddAfter(previous, new Node<T>(toAdd));
        }

        public void AddBefore(Node<T> next, Node<T> toAdd)
        {
            AddAfter(next.Previous, toAdd);
        }

        public void AddBefore(Node<T> next, T toAdd)
        {
            AddAfter(next.Previous, new Node<T>(toAdd));
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
            for (Node<T> i = head.Next; i != head; i = i.Next)
            {
                array[location] = i.Data;
                location++;
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
            return Remove(Find(toDelete));
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
            for (Node<T> i = head.Next; i != head; i = i.Next)
            {
                yield return i.Data;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private Node<T>? Find(T data)
        {
            for (Node<T> i = head.Next; i != head; i = i.Next)
            {
                if (i.Data.Equals(data))
                {
                    return i;
                }
            }

            return new Node<T>(default(T));
        }
    }
}
