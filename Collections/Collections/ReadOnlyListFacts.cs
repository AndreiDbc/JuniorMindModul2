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
    }
}
