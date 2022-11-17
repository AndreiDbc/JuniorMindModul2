using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linq
{
    class Stock
    {
        private readonly List<StockObject> stockObjects;
        private int count;

        public Stock()
        {
            stockObjects = new List<StockObject>();
            count = 0;
        }

        public int Count { get => count; }
    }
}
