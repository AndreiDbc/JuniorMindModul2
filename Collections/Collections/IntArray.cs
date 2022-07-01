using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collections
{
    class IntArray
    {
        private readonly int[] array;

        public IntArray()
        {
            this.array = new int[0];
        }

        public int Count()
        {
            return array.Length;
        }
    }
}
