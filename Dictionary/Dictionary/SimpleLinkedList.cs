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
        private Element<T> head;
        private int count;

        public SimpleLinkedList()
        {
            head = new Node<T>(default(T));
            head.Next = null;
            count = 0;
        }

        public int Count { get => count; }

        public Element<T> Last
        {
            get
            {
                for (Element<T> i = head; i != null; i = i.Next)
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
            Element<T> node = new Node<T>(data);
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
            Element<T> node = Find(toDelete);
            if (node == null)
            {
                return false;
            }

            Remove(node);
            return true;
        }

        public void Remove(Element<T> toDelete)
        {
            count--;
            if (toDelete.Equals(head))
            {
                head = toDelete.Next;
                return;
            }

            Element<T> previous = FindPreviousNode(toDelete);
            previous.Next = toDelete.Next;
        }

        public bool Contains(T data)
        {
            return Find(data) != null;
        }

        public void CopyTo(T[] array, int location)
        {
            for (Element<T> i = head; i != null; i = i.Next)
            {
                array[location] = i.Value;
                location++;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (Element<T> i = head; i != null; i = i.Next)
            {
                yield return i.Value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private Element<T>? Find(T data)
        {
            for (Element<T> i = head; i != null; i = i.Next)
            {
                if (i.Value.Equals(data))
                {
                    return i;
                }
            }

            return null;
        }

        private Element<T>? FindPreviousNode(Element<T> node)
        {
            for (Element<T> i = head; i != null; i = i.Next)
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
