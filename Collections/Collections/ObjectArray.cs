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

        public ObjectArray()
        {
            array = new object[4];
        }

        public int Count { get => array.Length; }

        public object this[int index]
        {
            get => array[index];
            set => array[index] = value;
        }

        public void Add(object element)
        {
            Array.Resize(ref array, array.Length * 2);
            array[GetLastValue() + 1] = element;
        }

        protected int GetLastValue()
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == null)
                {
                    return i - 1;
                }
            }

            return '\0';
        }
    }
}
