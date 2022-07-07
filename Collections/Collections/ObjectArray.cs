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
    }
}
