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

        public int Compare(T first, T second)
        {
            return new CompareFunc(Comparer<T>.Default.Compare).Invoke(first, second);
        }
    }
}
