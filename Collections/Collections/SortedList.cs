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

        private T ElementAt(int index, T defaultValue)
        {
            if (index == Count || index == -1)
            {
                return defaultValue;
            }

            return this[index];
        }
    }
}
