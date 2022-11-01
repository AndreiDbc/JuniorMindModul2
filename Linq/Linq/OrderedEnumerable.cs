using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linq
{
    class OrderedEnumerable<TElement> : IOrderedEnumerable<TElement>
    {
        private IEnumerable<TElement> source;

        public OrderedEnumerable(IEnumerable<TElement> source)
        {
            this.source = source;
        }

        public IOrderedEnumerable<TElement> CreateOrderedEnumerable<TKey>(Func<TElement, TKey> keySelector, IComparer<TKey>? comparer, bool descending)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<TElement> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
