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
            foreach (var number in array)
            {
                if (number == element)
                {
                    return true;
                }
            }

            return false;
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
            int[] temp = array;
            Array.Resize(ref array, array.Length + 1);
            array[index] = element;
            for (int i = index + 1; i < array.Length; i++)
            {
                array[i] = temp[i - 1];
            }
        }

        public void Clear()
        {
            array = new int[array.Length];
        }

        public void Remove(int element)
        {
            int[] temp = array;
            int index = IndexOf(element);
            if (index == -1)
            {
                return;
            }

            for (int i = index; i < array.Length - 1; i++)
            {
                array[i] = temp[i + 1];
            }

            Array.Resize(ref array, array.Length - 1);
        }

        public void RemoveAt(int index)
        {
            int[] temp = array;
            for (int i = index; i < array.Length - 1; i++)
            {
                array[i] = temp[i + 1];
            }

            Array.Resize(ref array, array.Length - 1);
        }
    }
}
