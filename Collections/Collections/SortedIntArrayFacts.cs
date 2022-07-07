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
        public void AnInstanceOfSortedArrayIsCreatedInsertIsCalledAndIndexIsZeroShouldReturnElementsInAscendingOrder()
        {
            SortedIntArray array = new ();
            array.Add(8);
            array.Add(9);
            array.Add(4);
            array.Add(11);
            array.Add(6);
            array.Insert(0, 2);
            Assert.Equal(2, array[0]);
            Assert.Equal(4, array[1]);
            Assert.Equal(6, array[2]);
            Assert.Equal(8, array[3]);
            Assert.Equal(9, array[4]);
            Assert.Equal(11, array[5]);
        }

        [Fact]
        public void AnInstanceOfSortedArrayIsCreatedInsertIsCalledAndIndexIsArrayMinusOneShouldReturnElementsInAscendingOrder()
        {
            SortedIntArray array = new ();
            array.Add(8);
            array.Add(9);
            array.Add(4);
            array.Add(11);
            array.Add(6);
            array.Insert(5, 34);
            Assert.Equal(4, array[0]);
            Assert.Equal(6, array[1]);
            Assert.Equal(8, array[2]);
            Assert.Equal(9, array[3]);
            Assert.Equal(11, array[4]);
            Assert.Equal(34, array[5]);
        }

        [Fact]
        public void AnInstanceOfSortedArrayIsCreatedInsertIsCalledAndElementIsNotInOrderShouldReturnArray()
        {
            SortedIntArray array = new ();
            array.Add(8);
            array.Add(9);
            array.Add(4);
            array.Add(11);
            array.Add(6);
            array.Insert(3, 55);
            Assert.Equal(4, array[0]);
            Assert.Equal(6, array[1]);
            Assert.Equal(8, array[2]);
            Assert.Equal(9, array[3]);
            Assert.Equal(11, array[4]);
        }

        [Fact]
        public void AnInstanceOfSortedArrayIsCreatedValuesAreSetUsingTheIndeexerShouldReturnSortedValues()
        {
            SortedIntArray array = new ();
            array.Add(8);
            array.Add(10);
            array.Add(4);
            array.Add(11);
            array.Add(6);
            array[1] = 19;
            array[3] = 9;
            Assert.Equal(4, array[0]);
            Assert.Equal(6, array[1]);
            Assert.Equal(8, array[2]);
            Assert.Equal(9, array[3]);
            Assert.Equal(11, array[4]);
        }
    }
}
