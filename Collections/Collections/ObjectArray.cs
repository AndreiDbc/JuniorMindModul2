using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collections
{
    class ObjectArray
    {
        private object[] array;
        private int count;

        public ObjectArray()
        {
            array = new object[4];
            count = 0;
        }

        public int Count { get => count; }

        public object this[int index]
        {
            get => array[index];
            set => array[index] = value;
        }

        public void Add(object element)
        {
            Array.Resize(ref array, array.Length * 2);
            array[GetLastValueIndex() + 1] = element;
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
