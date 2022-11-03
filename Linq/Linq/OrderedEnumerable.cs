using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linq
{
    class OrderedEnumerable<TKey, TElement> : IOrderedEnumerable<TElement>
    {
        private readonly IEnumerable<TElement> source;
        private readonly IComparer<TKey> currentComparer;
        private readonly Func<TElement, TKey> currentKeySelector;

        public OrderedEnumerable(IEnumerable<TElement> source, IComparer<TKey> currentComparer, Func<TElement, TKey> keySelector)
        {
            this.source = source;
            this.currentComparer = currentComparer;
            this.currentKeySelector = keySelector;
        }

        public IOrderedEnumerable<TElement> CreateOrderedEnumerable<TKey>(Func<TElement, TKey> keySelector, IComparer<TKey>? comparer, bool descending)
        {
            return new OrderedEnumerable<TKey, TElement>(source, comparer, keySelector);
        }

        public IEnumerator<TElement> GetEnumerator()
        {
            List<TElement> elements = source.ToList();
            while (elements.Count > 0)
            {
                TElement minElement = elements[0];
                int minIndex = 0;
                for (int i = 1; i < elements.Count; i++)
                {
                    if (currentComparer.Compare(currentKeySelector(elements[i]), currentKeySelector(minElement)) < 0)
                    {
                        minElement = elements[i];
                        minIndex = i;
                    }
                }

                elements.RemoveAt(minIndex);
                yield return minElement;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return source.GetEnumerator();
        }
    }
}
