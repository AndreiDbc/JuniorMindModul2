using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linq
{
    class Stock : IEnumerable<StockObject>
    {
        private readonly List<StockObject> stockObjects;
        private int count;

        public Stock()
        {
            stockObjects = new List<StockObject>();
            count = 0;
        }

        public int Count { get => count; }

        public IEnumerator<StockObject> GetEnumerator()
        {
            foreach (var item in stockObjects)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
