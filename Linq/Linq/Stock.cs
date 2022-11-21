using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linq
{
    class Stock : IEnumerable<Product>
    {
        private readonly List<Product> products;
        private readonly Action<Product> alert;
        private int count;

        public Stock(Action<Product> alert)
        {
            this.alert = alert;
            products = new List<Product>();
            count = 0;
        }

        public int Count { get => count; }

        public void Add(Product toAdd)
        {
            CheckForNull(toAdd);
            var index = Find(toAdd.Name);
            if (index == -1)
            {
                products.Add(toAdd);
                count++;
            }
            else
            {
                products[index].Quantity += toAdd.Quantity;
            }
        }

        public void RemoveQuantity(Product toRemove)
        {
            CheckForNull(toRemove);
            CheckIfContains(toRemove);
            var index = Find(toRemove.Name);
            var alertThresholds = new[] { 10, 5, 2 }.First(x => x > toRemove.Quantity);
            if (products[index].Quantity < toRemove.Quantity)
            {
                throw new ArgumentException("Not enough items in store!");
            }

            products[index].Quantity -= toRemove.Quantity;
            if (products[index].Quantity >= alertThresholds)
            {
                return;
            }

            alert(toRemove);
        }

        public IEnumerator<Product> GetEnumerator()
        {
            foreach (var item in products)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private static void CheckForNull(Product product)
        {
            if (product != null)
            {
                return;
            }

            throw new ArgumentException("Insert product name.");
        }

        private void CheckIfContains(Product product)
        {
            if (products.Any(val => val.Name == product.Name))
            {
                return;
            }

            throw new ArgumentException("Product not in store.");
        }

        private int Find(string name)
        {
            for (int i = 0; i < products.Count && products.Any(val => val.Name == name); i++)
            {
                if (products[i].Name == name)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}