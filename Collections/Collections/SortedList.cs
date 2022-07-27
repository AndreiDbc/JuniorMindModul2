using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collections
{
    class SortedList<T> : List<T>
        where T : IComparable<T>
    {
        public SortedList()
        {
        }

        public override T this[int index]
        {
            set
            {
                if (index >= Count)
                {
                    return;
                }

                if (ElementAt(index - 1, value).CompareTo(value) > 0 || value.CompareTo(ElementAt(index + 1, value)) > 0)
                {
                    return;
                }

                base[index] = value;
            }
        }

        public override void Add(T element)
        {
            base.Add(element);
            ShellSort();
        }

        public override void Insert(int index, T element)
        {
            if (ElementAt(index - 1, element).CompareTo(element) > 0 || element.CompareTo(ElementAt(index + 1, element)) > 0)
            {
                return;
            }

            base.Insert(index, element);
        }

        private T ElementAt(int index, T defaultValue)
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
            T temp;
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

        private void Swap(T temp, ref int j, int distance)
        {
            while ((j >= distance) && temp.CompareTo(base[j - distance]) < 0)
            {
                base[j] = base[j - distance];
                j -= distance;
            }
        }
    }
}
