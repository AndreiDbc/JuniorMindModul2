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
            QuickSort(0, GetLastValueIndex());
        }

        public override void Insert(int index, int element)
        {
            base.Insert(index, element);
            QuickSort(0, GetLastValueIndex());
        }

        public override void Remove(int element)
        {
            base.Remove(element);
            QuickSort(0, GetLastValueIndex());
        }

        public override void RemoveAt(int index)
        {
            base.RemoveAt(index);
            QuickSort(0, GetLastValueIndex());
        }

        private void QuickSort(int start, int end)
        {
            int i;
            if (start == end + 1)
            {
                return;
            }

            i = Partition(start, end);

            QuickSort(start, i - 1);
            QuickSort(i + 1, end);
        }

        private int Partition(int start, int end)
        {
            int temp;
            int endValue = array[end];
            int i = start - 1;

            for (int j = start; j <= end - 1; j++)
            {
                if (array[j] <= endValue)
                {
                    i++;
                    temp = array[i];
                    array[i] = array[j];
                    array[j] = temp;
                }
            }

            temp = array[i + 1];
            array[i + 1] = array[end];
            array[end] = temp;
            return i + 1;
        }
    }
}
