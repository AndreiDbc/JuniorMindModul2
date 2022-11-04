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
        private readonly IComparer<T> primary;
        private readonly IComparer<T> secondary;

        public CustomComparer(IComparer<T> primary, IComparer<T> secondary)
        {
            this.primary = primary;
            this.secondary = secondary;
        }

        public int Compare(T first, T second)
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
