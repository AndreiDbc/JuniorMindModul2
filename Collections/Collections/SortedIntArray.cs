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

        public override void Add(int element)
        {
            base.Add(element);
            ShellSort();
        }

        public override void Insert(int index, int element)
        {
            if (index == 0 && this[1] >= element)
            {
                base.Insert(index, element);
            }
            else if (index == this.Count && element >= this[index - 1])
            {
                base.Insert(index, element);
            }
            else if (this[index - 1] <= element && this[index + 1] >= element)
            {
                base.Insert(index, element);
            }
            else
            {
                return;
            }

            ShellSort();
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
                for (i = 0; i < this.Count; i++)
                {
                    j = i;
                    temp = this[i];
                    Swap(temp, ref j, distance);
                    this[j] = temp;
                }

                distance /= two;
            }
        }

        private void Swap(int temp, ref int j, int distance)
        {
            while ((j >= distance) && temp < this[j - distance])
            {
                this[j] = this[j - distance];
                j -= distance;
            }
        }
    }
}
