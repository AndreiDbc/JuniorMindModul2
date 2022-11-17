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

        public void Add(StockObject toAdd)
        {
            var index = Find(toAdd.Name);
            if (index == -1)
            {
                stockObjects.Add(toAdd);
                count++;
            }
            else
            {
                stockObjects[index].Quantity += toAdd.Quantity;
            }
        }

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

        private int Find(string name)
        {
            for (int i = 0; i < stockObjects.Count && stockObjects.Any(val => val.Name == name); i++)
            {
                if (stockObjects[i].Name == name)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
