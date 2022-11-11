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
        private readonly Func<T, T, int> compareFunc;

        public CustomComparer(Func<T, T, int> compareFunc)
        {
            this.compareFunc = compareFunc;
        }

        public int Compare(T first, T second)
        {
            return compareFunc(first, second);
        }
    }
}
