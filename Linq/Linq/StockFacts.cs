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
            var obj = new Product("bike", 2);
            var stock = new Stock(AlertForThreshold);
            stock.Add(obj);
            Assert.Equal(obj, stock.First(val => val.Equals(obj)));
        }

        [Fact]
        public void AddIsCalledStockContainsExistingObjectShouldUpdateQuantity()
        {
            var stock = new Stock(AlertForThreshold);
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
        public void AddIsCalledToAddIsNullShouldThrowException()
        {
            var stock = new Stock(AlertForThreshold);
            Assert.Throws<ArgumentException>(() => stock.Add(null));
        }

        [Fact]
        public void RemoveQuantityIsCalledShouldUpdateQuantity()
        {
            var stock = new Stock(AlertForThreshold);
            var result = new Product("bike", 17);
            stock.Add(new Product("bike", 20));
            stock.RemoveQuantity(new Product("bike", 3));
            IEnumerator<Product> enumerator = stock.GetEnumerator();
            enumerator.MoveNext();
            Assert.Equal(result.Name, enumerator.Current.Name);
            Assert.Equal(result.Quantity, enumerator.Current.Quantity);
        }

        [Fact]
        public void RemoveQuantityIsCalledToRemoveIsNullShouldThrowException()
        {
            var stock = new Stock(AlertForThreshold);
            Assert.Throws<ArgumentException>(() => stock.RemoveQuantity(null));
        }

        [Fact]
        public void RemoveQuantityIsCalledItemIsNotInStoreSHouldThrowException()
        {
            var stock = new Stock(AlertForThreshold);
            stock.Add(new Product("bike", 15));
            Assert.Throws<ArgumentException>(() => stock.RemoveQuantity(new Product("hat", 2)));
        }

        [Fact]
        public void RemoveIsCalledToRemoveQuantityIsGreaterThanExistingQuantityShouldNotUpdateResult()
        {
            var stock = new Stock(AlertForThreshold);
            var result = new Product("bike", 2);
            stock.Add(new Product("bike", 2));
            Assert.Throws<ArgumentException>(() => stock.RemoveQuantity(new Product("bike", 5)));
        }

        [Fact]
        public void RemoveIsCalledShouldNotifyForLessThanTenItems()
        {
            var stock = new Stock(Alert);
            stock.Add(new Product("bike", 2));
            bool alertTriggered = false;
            void Alert(Product product)
            {
                alertTriggered = true;
            }

            stock.RemoveQuantity(new Product("bike", 1));
            Assert.True(alertTriggered);
        }

        private static void AlertForThreshold(Product product)
        {
            if (product.Quantity == 1)
            {
                throw new ArgumentException("Only one " + product.Name + " left!");
            }

            throw new ArgumentException("Only " + product.Quantity + " " + product.Name + "s left!");
        }
    }
}
