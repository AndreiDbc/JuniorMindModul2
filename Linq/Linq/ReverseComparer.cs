using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linq
{
    class ReverseComparer<T> : IComparer<T>
    {
        private readonly IComparer<T> forwardComparer;

        public ReverseComparer(IComparer<T> forwardComparer)
        {
            this.forwardComparer = forwardComparer;
        }

        public int Compare(T first, T second)
        {
            return forwardComparer.Compare(second, first);
        }
    }
}
