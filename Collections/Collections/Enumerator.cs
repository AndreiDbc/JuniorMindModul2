using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collections
{
    class Enumerator : IEnumerator
    {
        private readonly object[] array;
        int position = -1;

        public Enumerator(object[] list)
        {
            array = list;
        }

        object IEnumerator.Current
        {
            get
            {
                return array[position];
            }
        }

        public bool MoveNext()
        {
            position++;
            return position < array.Length;
        }
    }
}
