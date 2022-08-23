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
        private T[] array;
        private int count;

        public LinkedList()
        {
            array = new T[4];
            count = 0;
        }

        public int Count { get => count; }

        public bool IsReadOnly { get; }

        public virtual T this[int index]
        {
            get => array[index];
            set
            {
                if (index < 0 || index >= count)
                {
                    throw new ArgumentException("Index was outside the bounds of the array");
                }

                array[index] = value;
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
