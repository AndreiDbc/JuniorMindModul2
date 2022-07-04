using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collections
{
    class IntArray
    {
        protected int[] array;
        protected int countValues;

        public IntArray()
        {
            this.array = new int[4];
            this.countValues = 0;
        }

        public int Count { get => array.Length; }

        public int this[int index]
        {
            get => array[index];
            set => array[index] = value;
        }

        public virtual void Add(int element)
        {
            Array.Resize(ref array, array.Length * 2);
            array[countValues] = element;
            countValues++;
        }

        public bool Contains(int element)
        {
            return IndexOf(element) != -1;
        }

        public int IndexOf(int element)
        {
            for (int i = 0; i < countValues - 1; i++)
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
            Array.Resize(ref array, array.Length + 1);
            ShiftToRight(index);
            array[index] = element;
            countValues++;
        }

        public void Clear()
        {
            array = new int[4];
            countValues = 0;
        }

        public virtual void Remove(int element)
        {
            int index = IndexOf(element);
            if (index == -1)
            {
                return;
            }

            ShiftToLeft(index);
            RemoveLastValue();
            countValues--;
        }

        public virtual void RemoveAt(int index)
        {
            ShiftToLeft(index);
            RemoveLastValue();
            countValues--;
        }

        private void RemoveLastValue()
        {
            array[countValues - 1] = '\0';
        }

        private void ShiftToRight(int index)
        {
            for (int i = countValues; i > index; i--)
            {
                array[i] = array[i - 1];
            }
        }

        private void ShiftToLeft(int index)
        {
            for (int i = index; i < countValues - 1; i++)
            {
                array[i] = array[i + 1];
            }
        }
    }
}
