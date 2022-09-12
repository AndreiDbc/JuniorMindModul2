using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionary
{
    class Dictionary<T>
    {
        private readonly int[] buckets;
        private readonly SimpleLinkedList<T>[] elements;

        public Dictionary()
        {
            buckets = new int[5];
            elements = new SimpleLinkedList<T>[5];
        }
    }
}
