using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Linq
{
    class CustomComparer<T> : IComparer<T>
    {
        private readonly CompareFunc compareFunc;

        public CustomComparer(CompareFunc compareFunc)
        {
            this.compareFunc = compareFunc;
        }

        public delegate int CompareFunc(T first, T second);

        public int Compare(TElement first, TElement second)
        {
            if (keySelector != null)
            {
                return CompareKeys(first, second);
            }
            else if (secondary != null)
            {
                return ComparePrimaryAndSecondary(first, second);
            }

            return ReverseCompare(first, second);
        }

        private int ComparePrimaryAndSecondary(TElement first, TElement second)
        {
            int primaryResult = primary.Compare(first, second);
            if (primaryResult != 0)
            {
                return primaryResult;
            }

            return secondary.Compare(first, second);
        }

        private int CompareKeys(TElement first, TElement second)
        {
            TKey firstKey = keySelector(first);
            TKey secondKey = keySelector(second);
            return keyComparer.Compare(firstKey, secondKey);
        }

        private int ReverseCompare(TElement first, TElement second)
        {
            return primary.Compare(second, first);
        }
    }
}
