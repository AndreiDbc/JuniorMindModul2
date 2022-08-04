using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Collections
{
    public class ReadOnlyListFacts
    {
        [Fact]
        public void AnInstanceOfReadOnlyListIsCreatedSHouldReturnElements()
        {
            List<int> list = new List<int>() { 1, 2, 3, 4 };
            ReadOnlyList<int> test = new ReadOnlyList<int>(list);
            Assert.Equal(1, test[0]);
            Assert.Equal(2, test[1]);
            Assert.Equal(3, test[2]);
            Assert.Equal(4, test[3]);
        }

        [Fact]
        public void AnInstanceOfReadOnlyListIsCreatedValueIsSetUsingIndeexerShoudThrowNotSupportedException()
        {
            List<int> list = new List<int>() { 1, 2, 3, 4 };
            ReadOnlyList<int> test = new ReadOnlyList<int>(list);
            var message = Assert.Throws<NotSupportedException>(() => test[0] = 34);
            Assert.Contains("List is Readonly.", message.Message);
        }

        [Fact]
        public void AninstanceOfReadOnlyListIsCreatedAddIsCalledShouldThrowNotSupportedException()
        {
            List<int> list = new List<int>() { 1, 2, 3, 4 };
            ReadOnlyList<int> test = new ReadOnlyList<int>(list);
            var message = Assert.Throws<NotSupportedException>(() => test.Add(5));
            Assert.Contains("List is Readonly.", message.Message);
        }

        [Fact]
        public void AninstanceOfReadOnlyListIsCreatedRemoveIsCalledShouldThrowNotSupportedException()
        {
            List<int> list = new List<int>() { 1, 2, 3, 4 };
            ReadOnlyList<int> test = new ReadOnlyList<int>(list);
            var message = Assert.Throws<NotSupportedException>(() => test.Remove(1));
            Assert.Contains("List is Readonly.", message.Message);
        }

        [Fact]
        public void AninstanceOfReadOnlyListIsCreatedRemoveAtIsCalledShouldThrowNotSupportedException()
        {
            List<int> list = new List<int>() { 1, 2, 3, 4 };
            ReadOnlyList<int> test = new ReadOnlyList<int>(list);
            var message = Assert.Throws<NotSupportedException>(() => test.RemoveAt(0));
            Assert.Contains("List is Readonly.", message.Message);
        }

        [Fact]
        public void AninstanceOfReadOnlyListIsCreatedInsertIsCalledShouldThrowNotSupportedException()
        {
            List<int> list = new List<int>() { 1, 2, 3, 4 };
            ReadOnlyList<int> test = new ReadOnlyList<int>(list);
            var message = Assert.Throws<NotSupportedException>(() => test.Insert(0, 1));
            Assert.Contains("List is Readonly.", message.Message);
        }

        [Fact]
        public void AninstanceOfReadOnlyListIsCreatedClearIsCalledShouldThrowNotSupportedException()
        {
            List<int> list = new List<int>() { 1, 2, 3, 4 };
            ReadOnlyList<int> test = new ReadOnlyList<int>(list);
            var message = Assert.Throws<NotSupportedException>(() => test.Clear());
            Assert.Contains("List is Readonly.", message.Message);
        }

        [Fact]
        public void AnInstanceOfReadOnlyListIsCreatedCountIsCalledShouldReturnNoOfElements()
        {
            List<int> list = new List<int>();
            list.Add(1);
            list.Add(2);
            list.Add(3);
            list.Add(4);
            ReadOnlyList<int> test = new ReadOnlyList<int>(list);
            Assert.Equal(4, test.Count);
        }

        [Fact]
        public void AnInstanceOfReadOnlyListIsCreatedCopyToIsCalledShouldCopyElementsToDestinationTarget()
        {
            List<int> list = new List<int>();
            list.Add(1);
            list.Add(2);
            list.Add(3);
            list.Add(4);
            ReadOnlyList<int> test = new ReadOnlyList<int>(list);
            int[] arr = new int[4];
            test.CopyTo(arr, 0);
            Assert.Equal(1, arr[0]);
            Assert.Equal(2, arr[1]);
            Assert.Equal(3, arr[2]);
            Assert.Equal(4, arr[3]);
        }

        [Fact]
        public void AnInstanceOfReadOnlyListIsCreatedContainsIsCalledShouldReturnTrue()
        {
            List<int> list = new List<int>();
            list.Add(1);
            list.Add(2);
            list.Add(3);
            list.Add(4);
            ReadOnlyList<int> test = new ReadOnlyList<int>(list);
            Assert.True(test.Contains(2));
        }

        [Fact]
        public void AnInstanceOfReadOnlyListIsCreatedContainsIsCalledShouldReturnFalse()
        {
            List<int> list = new List<int>();
            list.Add(1);
            list.Add(2);
            list.Add(3);
            list.Add(4);
            ReadOnlyList<int> test = new ReadOnlyList<int>(list);
            Assert.False(test.Contains(34));
        }

        [Fact]
        public void AnInstanceOfReadOnlyListIsCreatedIndexOfCalledShouldReturnIndex()
        {
            List<int> list = new List<int>();
            list.Add(1);
            list.Add(2);
            list.Add(3);
            list.Add(4);
            ReadOnlyList<int> test = new ReadOnlyList<int>(list);
            Assert.Equal(1, test.IndexOf(2));
        }

        [Fact]
        public void GetEnumeratorMoveNextIsCalledShouldReturnTrue()
        {
            var array = new List<string>();
            array.Add("asd");
            array.Add("gfs");
            array.Add("AS");
            ReadOnlyList<string> test = new ReadOnlyList<string>(array);
            Assert.True(test.GetEnumerator().MoveNext());
        }

        [Fact]
        public void GetEnumeratorMoveNextIsCalledShouldReturnNextElement()
        {
            var array = new List<string>();
            array.Add("asd");
            array.Add("gfs");
            array.Add("AS");
            ReadOnlyList<string> test = new ReadOnlyList<string>(array);
            IEnumerator<string> enumerator = test.GetEnumerator();
            enumerator.MoveNext();
            enumerator.MoveNext();
            Assert.Equal(test[1], enumerator.Current);
        }
    }
}
