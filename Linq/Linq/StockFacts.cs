using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Linq
{
    public class StockFacts
    {
        [Fact]
        public void AddIsCalledShouldAddItem()
        {
            var stock = new Stock();
            var obj = new StockObject("bike", 2);
            stock.Add(obj);
            Assert.True(stock.Any(val => val.Equals(obj)));
        }

        [Fact]
        public void AddIsCalledStockContainsExistingObjectShouldUpdateQuantity()
        {
            var stock = new Stock();
            stock.Add(new StockObject("bike", 2));
            stock.Add(new StockObject("bike", 2));
            Assert.Equal(new StockObject("bike", 4), stock.First());
            Assert.Equal(1, stock.Count);
        }
    }
}
