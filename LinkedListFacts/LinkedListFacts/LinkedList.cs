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
            head = null;
            count = 0;
        }

        public int Count { get => count; }

        public bool IsReadOnly { get; }

        public Node<T> First { get => head; }

        public Node<T> Last
        {
            get
            {
                var temp = head;
                for (int i = 0; i < Count; i++)
                {
                    temp = temp.Next;
                }

                return temp;
            }
        }

        public virtual void Add(T element)
        {
            DoubleTheArraySize();
            array[count] = element;
            count++;
        }

        public void CopyTo(T[] arrayToAdd, int arrayIndex)
        {
            if (arrayToAdd == null)
            {
                throw new ArgumentNullException(nameof(arrayToAdd));
            }
            else if (arrayIndex < 0 || arrayIndex >= Count)
            {
                throw new ArgumentException("Index is outside of the bounds of the array.");
            }
            else if (Count > arrayToAdd.Length - arrayIndex)
            {
                throw new ArgumentException("The number of elements array is greater than the available space to the end of the destination array.");
            }

            Array.Copy(array, 0, arrayToAdd, arrayIndex, Count);
        }

        public bool Contains(T element)
        {
            return IndexOf(element) != -1;
        }

        public void Clear()
        {
            count = 0;
        }

        public bool Remove(T element)
        {
            int index = IndexOf(element);
            if (index == -1)
            {
                return false;
            }

            ShiftToLeft(index);
            count--;

            return true;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int index = 0; index < count; index++)
            {
                yield return array[index];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void ShiftToRight(int index)
        {
            for (int i = count; i > index; i--)
            {
                array[i] = array[i - 1];
            }
        }

        private void DoubleTheArraySize()
        {
            if (count != array.Length)
            {
                return;
            }

            Array.Resize(ref array, array.Length * 2);
        }

        private void ShiftToLeft(int index)
        {
            for (int i = index; i < count - 1; i++)
            {
                array[i] = array[i + 1];
            }
        }
    }
}
