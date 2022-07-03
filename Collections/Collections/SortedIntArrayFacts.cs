using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Collections
{
    public class SortedIntArrayFacts
    {
        [Fact]
        public void AnInstanceOfSortedArrayIsCreatedAddIsCalledShouldReturnElementsInAscendingOrder()
        {
            SortedIntArray array = new ();
            array.Add(8);
            array.Add(9);
            array.Add(4);
            array.Add(11);
            array.Add(6);
            Assert.Equal(4, array[0]);
            Assert.Equal(6, array[1]);
            Assert.Equal(8, array[2]);
            Assert.Equal(9, array[3]);
            Assert.Equal(11, array[4]);
        }

        [Fact]
        public void AnInstanceOfSortedArrayIsCreatedInsertIsCalledShouldReturnElementsInAscendingOrder()
        {
            SortedIntArray array = new ();
            array.Add(8);
            array.Add(9);
            array.Add(4);
            array.Add(11);
            array.Add(6);
            array.Insert(1, 5);
            Assert.Equal(4, array[0]);
            Assert.Equal(5, array[1]);
            Assert.Equal(6, array[2]);
            Assert.Equal(8, array[3]);
            Assert.Equal(9, array[4]);
            Assert.Equal(11, array[5]);
        }

        [Fact]
        public void AnInstanceOfSortedArrayIsCreatedRemoveIsCalledShouldReturnElementsInAscendingOrder()
        {
            SortedIntArray array = new ();
            array.Add(8);
            array.Add(9);
            array.Add(4);
            array.Add(9);
            array.Add(6);
            array.Remove(3);
            Assert.Equal(4, array[0]);
            Assert.Equal(6, array[1]);
            Assert.Equal(8, array[2]);
            Assert.Equal(9, array[3]);
        }

        [Fact]
        public void AnInstanceOfSortedArrayIsCreatedRemoveAtIsCalledShouldReturnElementsInAscendingOrder()
        {
            SortedIntArray array = new ();
            array.Add(8);
            array.Add(9);
            array.Add(4);
            array.Add(11);
            array.Add(6);
            array.RemoveAt(2);
            Assert.Equal(4, array[0]);
            Assert.Equal(6, array[1]);
            Assert.Equal(9, array[2]);
            Assert.Equal(11, array[3]);
        }
    }
}
