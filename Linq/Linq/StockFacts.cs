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
            Assert.Equal(obj, stock.First(val => val.Equals(obj)));
        }

        [Fact]
        public void AddIsCalledStockContainsExistingObjectShouldUpdateQuantity()
        {
            var stock = new Stock();
            var obj = new StockObject("bike", 2);
            var result = new StockObject("bike", 4);
            stock.Add(obj);
            stock.Add(obj);
            Assert.Equal(result, stock.First(val => val.Equals(result)));
            Assert.Equal(1, stock.Count);
        }

        [Fact]
        public void RemoveQuantityIsCalledShouldUpdateQuantity()
        {
            var stock = new Stock();
            var result = new StockObject("bike", 1);
            stock.Add(new StockObject("bike", 2));
            stock.RemoveQuantity(new StockObject("bike", 1));
            Assert.Equal(result, stock.First(val => val.Equals(result)));
        }

        [Fact]
        public void RemoveIsCalledToRemoveQuantityIsGreaterThanExistingQuantityShouldNotUpdateResult()
        {
            var stock = new Stock();
            var result = new StockObject("bike", 2);
            stock.Add(new StockObject("bike", 2));
            stock.RemoveQuantity(new StockObject("bike", 5));
            Assert.Equal(result, stock.First(val => val.Equals(result)));
        }
    }
}
