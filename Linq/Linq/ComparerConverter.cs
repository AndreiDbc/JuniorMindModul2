using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linq
{
    class ComparerConverter<TElement, TKey> : IComparer<TElement>
    {
        private readonly Func<TElement, TKey> keySelector;
        private readonly IComparer<TKey> comparer;

        public ComparerConverter(Func<TElement, TKey> keySelector, IComparer<TKey> comparer)
        {
            this.keySelector = keySelector;
            this.comparer = comparer;
        }

        public int Compare(TElement first, TElement second)
        {
            TKey firstKey = keySelector(first);
            TKey SecondKey = keySelector(second);
            return comparer.Compare(firstKey, SecondKey);
        }
    }
}
