using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedListFacts
{
    class LinkedList<T>
    {
        private T[] array;
        private int count;

        public LinkedList()
        {
            array = new T[4];
            count = 0;
        }

        public int Count { get => count; }
    }
}
