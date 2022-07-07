using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collections
{
    class SortedIntArray : IntArray
    {
        public SortedIntArray()
        {
        }

        public override int this[int index]
        {
            set
            {
                if (ElementAt(index - 1, value - 1) > value || value > ElementAt(index + 1, value))
                {
                    return;
                }

                base[index] = value;
            }
        }

        public override void Add(int element)
        {
            base.Add(element);
            ShellSort();
        }

        public override void Insert(int index, int element)
        {
            if (ElementAt(index - 1, element) > element || element > ElementAt(index, element))
            {
                return;
            }

            base.Insert(index, element);
        }

        public override void Clear()
        {
            base.Clear();
            for (int i = 0; i < Count; i++)
            {
                this[i] = 0;
            }
        }

        private int ElementAt(int index, int defaultValue)
        {
            if (index == Count || index == -1)
            {
                return defaultValue;
            }

            return this[index];
        }

        private void ShellSort()
        {
            const int two = 2;
            int i;
            int j;
            int temp;
            int distance = 3;
            while (distance > 0)
            {
                for (i = 0; i < Count; i++)
                {
                    j = i;
                    temp = base[i];
                    Swap(temp, ref j, distance);
                    base[j] = temp;
                }

                distance /= two;
            }
        }

        private void Swap(int temp, ref int j, int distance)
        {
            while ((j >= distance) && temp < base[j - distance])
            {
                base[j] = base[j - distance];
                j -= distance;
            }
        }
    }
}
