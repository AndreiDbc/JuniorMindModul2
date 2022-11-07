using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Linq
{
    class CustomComparer<TElement, TKey> : IComparer<TElement>
    {
        private readonly Func<TElement, TKey> keySelector;
        private readonly IComparer<TElement> primary;
        private readonly IComparer<TElement> secondary;
        private readonly IComparer<TKey> keyComparer;

        public CustomComparer(IComparer<TElement> primary, IComparer<TElement> secondary)
        {
            this.primary = primary;
            this.secondary = secondary;
        }

        public CustomComparer(Func<TElement, TKey> keySelector, IComparer<TKey> keyComparer)
        {
            this.keySelector = keySelector;
            this.keyComparer = keyComparer;
        }

        public CustomComparer(IComparer<TElement> primary)
        {
            this.primary = primary;
        }

        public int ComparePrimaryAndSecondary(TElement first, TElement second)
        {
            int primaryResult = primary.Compare(first, second);
            if (primaryResult != 0)
            {
                return primaryResult;
            }

            return secondary.Compare(first, second);
        }
    }
}
