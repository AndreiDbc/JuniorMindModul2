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
        public void AnInstanceOfReadOnlyListIsCreatedValueIsSetUsingIndeexerShoudThrowNotSupportedException()
        {
            ReadOnlyList<int> test = new ReadOnlyList<int>();
            var message = Assert.Throws<NotSupportedException>(() => test[0] = 34);
            Assert.Contains("List is Readonly.", message.Message);
        }

        [Fact]
        public void AninstanceOfReadOnlyListIsCreatedAddIsCalledShouldThrowNotSupportedException()
        {
            ReadOnlyList<int> test = new ReadOnlyList<int>();
            var message = Assert.Throws<NotSupportedException>(() => test.Add(5));
            Assert.Contains("List is Readonly.", message.Message);
        }

        [Fact]
        public void AninstanceOfReadOnlyListIsCreatedRemoveIsCalledShouldThrowNotSupportedException()
        {
            ReadOnlyList<int> test = new ReadOnlyList<int>();
            var message = Assert.Throws<NotSupportedException>(() => test.Remove(1));
            Assert.Contains("List is Readonly.", message.Message);
        }

        [Fact]
        public void AninstanceOfReadOnlyListIsCreatedRemoveAtIsCalledShouldThrowNotSupportedException()
        {
            ReadOnlyList<int> test = new ReadOnlyList<int>();
            var message = Assert.Throws<NotSupportedException>(() => test.RemoveAt(0));
            Assert.Contains("List is Readonly.", message.Message);
        }

        [Fact]
        public void AninstanceOfReadOnlyListIsCreatedInsertIsCalledShouldThrowNotSupportedException()
        {
            ReadOnlyList<int> test = new ReadOnlyList<int>();
            var message = Assert.Throws<NotSupportedException>(() => test.Insert(0, 1));
            Assert.Contains("List is Readonly.", message.Message);
        }

        [Fact]
        public void AninstanceOfReadOnlyListIsCreatedClearIsCalledShouldThrowNotSupportedException()
        {
            ReadOnlyList<int> test = new ReadOnlyList<int>();
            var message = Assert.Throws<NotSupportedException>(() => test.Clear());
            Assert.Contains("List is Readonly.", message.Message);
        }
    }
}
