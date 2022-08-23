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
