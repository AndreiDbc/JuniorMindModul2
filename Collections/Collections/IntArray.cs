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
            this.array = new int[4];
        }

        public int Count { get => array.Length; }

        public int this[int index]
        {
            get => array[index];
            set => array[index] = value;
        }

        public void Add(int element)
        {
            Array.Resize(ref array, array.Length * 2);
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == '\0')
                {
                    array[i] = element;
                    break;
                }
            }
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
            array = new int[4];
        }

        public void Remove(int element)
        {
            int index = IndexOf(element);
            if (index == -1)
            {
                return;
            }

            ShiftToLeft(index);
        }

        public void RemoveAt(int index)
        {
            ShiftToRight(index);
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
