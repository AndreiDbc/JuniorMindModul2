using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionary
{
    class SimpleLinkedList<T> : ICollection<T>
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
                for (Node<T> i = head; i != null; i = i.Next)
                {
                    if (i.Next == null)
                    {
                        return i;
                    }
                }

                return head;
            }
        }

        public bool IsReadOnly { get; }

        public void Add(T data)
        {
            Node<T> node = new Node<T>(data);
            node.Next = count == 0 ? null : head;
            head = node;
            count++;
        }

        public void Clear()
        {
            head = new Node<T>(default(T));
            head.Next = null;
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
            count--;
            if (toDelete.Equals(head))
            {
                head = toDelete.Next;
                return;
            }

            Node<T> previous = FindPreviousNode(toDelete);
            previous.Next = toDelete.Next;
        }

        public bool Contains(T data)
        {
            return Find(data) != null;
        }

        public void CopyTo(T[] array, int location)
        {
            for (Node<T> i = head; i != null; i = i.Next)
            {
                array[location] = i.Data;
                location++;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (Node<T> i = head; i != null; i = i.Next)
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
            for (Node<T> i = head; i != null; i = i.Next)
            {
                if (i.Data.Equals(data))
                {
                    return i;
                }
            }

            return null;
        }

        private Node<T>? FindPreviousNode(Node<T> node)
        {
            for (Node<T> i = head; i != null; i = i.Next)
            {
                if (i.Next == node)
                {
                    return i;
                }
            }

            return null;
        }
    }
}
