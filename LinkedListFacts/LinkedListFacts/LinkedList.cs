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
            head.IsContained = this;
            count = 0;
        }

        public int Count { get => count; }

        public bool IsReadOnly { get; }

        public Node<T> Last { get => count == 0 ? null : head.Previous; }

        public Node<T> First { get => count == 0 ? null : head.Next; }

        public void Add(T toAdd)
        {
            AddAfter(head.Previous, toAdd);
        }

        public void AddAfter(Node<T> previous, Node<T> toAdd)
        {
            CheckArgumentNullException(toAdd);
            CheckArgumentNullException(previous);
            CheckInvalidOperationException(previous);

            toAdd.Next = previous.Next;
            previous.Next = toAdd;
            toAdd.Previous = previous;
            toAdd.Next.Previous = toAdd;
            toAdd.IsContained = this;
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
            AddBefore(next, new Node<T>(toAdd));
        }

        public void AddFirst(Node<T> toAdd)
        {
            AddAfter(head, toAdd);
        }

        public void AddFirst(T toAdd)
        {
            AddAfter(head, toAdd);
        }

        public void AddLast(Node<T> toAdd)
        {
            AddAfter(head.Previous, toAdd);
        }

        public void AddLast(T toAdd)
        {
            AddAfter(Last, toAdd);
        }

        public bool Contains(T data)
        {
            return Find(data) != null;
        }

        public void CopyTo(T[] array, int location)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }
            else if (location < 0)
            {
                throw new ArgumentException("Index is outside of the bounds of the array.");
            }
            else if (Count > array.Length - location)
            {
                throw new ArgumentException("The number of elements array is greater than the available space to the end of the destination array.");
            }

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
            count = 0;
        }

        public bool Remove(T toDelete)
        {
            Node<T> node = Find(toDelete);
            if (node == null)
            {
                return false;
            }

            Remove(node);
            return true;
        }

        public void Remove(Node<T> toDelete)
        {
            CheckArgumentNullException(toDelete);
            CheckInvalidOperationException(toDelete);

            var temp = toDelete.Next;
            temp.Previous = toDelete.Previous;
            toDelete.Next = temp;
            toDelete.Previous.Next = temp;
            toDelete.IsContained = null;
            count--;
        }

        public void RemoveFirst()
        {
            Remove(head.Next);
        }

        public void RemoveLast()
        {
            Remove(head.Previous);
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

        private static void CheckArgumentNullException(Node<T> node)
        {
            if (node != null)
            {
                return;
            }

            throw new ArgumentNullException(nameof(node));
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

            return null;
        }

        private void CheckInvalidOperationException(Node<T> node)
        {
            if (Contains(node.Data) || node == head)
            {
                return;
            }

            throw new InvalidOperationException("Node is not in the current LinkedList<T>.");
        }
    }
}
