using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collections
{
    class IntArray
    {
        private int[] array;
        private int count;

        public IntArray()
        {
            this.array = new int[4];
            this.count = 0;
        }

        public int Count { get => count; }

        public int this[int index]
        {
            get => array[index];
            set => array[index] = value;
        }

        public virtual void Add(int element)
        {
            DoubleTheArraySize();

            array[count] = element;
            count++;
        }

        public bool Contains(int element)
        {
            return IndexOf(element) != -1;
        }

        public int IndexOf(int element)
        {
            for (int i = 0; i < count; i++)
            {
                if (array[i] == element)
                {
                    return i;
                }
            }

            return -1;
        }

        public virtual void Insert(int index, int element)
        {
            DoubleTheArraySize();

            ShiftToRight(index);
            array[index] = element;
            count++;
        }

        public virtual void Clear()
        {
            count = 0;
        }

        public virtual void Remove(int element)
        {
            int index = IndexOf(element);
            if (index == -1)
            {
                return;
            }

            ShiftToLeft(index);
            count--;
        }

        public virtual void RemoveAt(int index)
        {
            ShiftToLeft(index);
            count--;
        }

        private void ShiftToRight(int index)
        {
            for (int i = count; i > index; i--)
            {
                array[i] = array[i - 1];
            }
        }

        private void ShiftToLeft(int index)
        {
            for (int i = index; i < count - 1; i++)
            {
                array[i] = array[i + 1];
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
    }
}
