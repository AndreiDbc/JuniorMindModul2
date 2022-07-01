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

        public IntArray()
        {
            this.array = new int[0];
        }

        public int Count()
        {
            return array.Length;
        }

        public int Element(int index)
        {
            return array[index];
        }

        public void Add(int element)
        {
            Array.Resize(ref array, array.Length + 1);
            array[^1] = element;
        }

        public void SetElement(int index, int element)
        {
            array[index] = element;
        }

        public bool Contains(int element)
        {
            return IndexOf(element) != -1;
        }

        public int IndexOf(int element)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == element)
                {
                    return i;
                }
            }

            return -1;
        }

        public void Insert(int index, int element)
        {
            Array.Resize(ref array, array.Length + 1);
            ShiftToRight(index);
            array[index] = element;
        }

        public void Clear()
        {
            array = new int[0];
        }

        public void Remove(int element)
        {
            int index = IndexOf(element);
            if (index == -1)
            {
                return;
            }

            ShiftToLeft(index);
            Array.Resize(ref array, array.Length - 1);
        }

        public void RemoveAt(int index)
        {
            ShiftToRight(index);
            Array.Resize(ref array, array.Length - 1);
        }

        public void ShiftToRight(int index)
        {
            for (int i = array.Length - 1; i > index; i--)
            {
                array[i] = array[i - 1];
            }
        }

        public void ShiftToLeft(int index)
        {
            for (int i = index; i < array.Length - 1; i++)
            {
                array[i] = array[i + 1];
            }
        }
    }
}
