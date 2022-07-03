using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collections
{
    class ObjectArray
    {
        private readonly object[] array;

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
    }
}
