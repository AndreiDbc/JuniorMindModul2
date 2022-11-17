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
            var obj = new Product("bike", 2);
            stock.Add(obj);
            Assert.Equal(obj, stock.First(val => val.Equals(obj)));
        }

        [Fact]
        public void AddIsCalledStockContainsExistingObjectShouldUpdateQuantity()
        {
            var stock = new Stock();
            var obj = new Product("bike", 2);
            var result = new Product("bike", 4);
            stock.Add(obj);
            stock.Add(obj);
            IEnumerator<Product> enumerator = stock.GetEnumerator();
            enumerator.MoveNext();
            Assert.Equal(result.Name, enumerator.Current.Name);
            Assert.Equal(result.Quantity, enumerator.Current.Quantity);
            Assert.Equal(1, stock.Count);
        }

        [Fact]
        public void RemoveQuantityIsCalledShouldUpdateQuantity()
        {
            var stock = new Stock();
            var result = new Product("bike", 1);
            stock.Add(new Product("bike", 2));
            stock.RemoveQuantity(new Product("bike", 1));
            IEnumerator<Product> enumerator = stock.GetEnumerator();
            enumerator.MoveNext();
            Assert.Equal(result.Name, enumerator.Current.Name);
            Assert.Equal(result.Quantity, enumerator.Current.Quantity);
        }

        [Fact]
        public void RemoveIsCalledToRemoveQuantityIsGreaterThanExistingQuantityShouldNotUpdateResult()
        {
            var stock = new Stock();
            var result = new Product("bike", 2);
            stock.Add(new Product("bike", 2));
            stock.RemoveQuantity(new Product("bike", 5));
            IEnumerator<Product> enumerator = stock.GetEnumerator();
            enumerator.MoveNext();
            Assert.Equal(result.Name, enumerator.Current.Name);
            Assert.Equal(result.Quantity, enumerator.Current.Quantity);
        }
    }
}
