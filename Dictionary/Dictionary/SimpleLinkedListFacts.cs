using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Dictionary
{
    public class SimpleLinkedListFacts
    {
        [Fact]
        public void AddIsCalledShouldAddOneElementToList()
        {
            SimpleLinkedList<int> list = new SimpleLinkedList<int>();
            list.Add(1);
            IEnumerator<int> enumerator = list.GetEnumerator();
            enumerator.MoveNext();
            Assert.Equal(1, enumerator.Current);
            Assert.False(enumerator.MoveNext());
            Assert.Equal(null, list.Last.Next);
        }

        [Fact]
        public void AddIsCalledMultipleTimesShouldUpdateList()
        {
            SimpleLinkedList<int> list = new SimpleLinkedList<int>();
            list.Add(1);
            list.Add(2);
            list.Add(3);
            IEnumerator<int> enumerator = list.GetEnumerator();
            enumerator.MoveNext();
            Assert.Equal(3, enumerator.Current);
            enumerator.MoveNext();
            Assert.Equal(2, enumerator.Current);
            enumerator.MoveNext();
            Assert.Equal(1, enumerator.Current);
            Assert.False(enumerator.MoveNext());
            Assert.Equal(null, list.Last.Next);
        }

        [Fact]
        public void ClearIsCalledShouldRevertListToInitialForm()
        {
            SimpleLinkedList<int> list = new SimpleLinkedList<int>();
            list.Add(1);
            list.Add(2);
            list.Add(3);
            list.Clear();
            IEnumerator<int> enumerator = list.GetEnumerator();
            Assert.Equal(0, list.Count);
            enumerator.MoveNext();
            Assert.Equal(0, enumerator.Current);
            Assert.False(enumerator.MoveNext());
        }

        [Fact]
        public void RemoveIsCalledShouldDeleteNodeContainingData()
        {
            SimpleLinkedList<int> list = new SimpleLinkedList<int>();
            list.Add(1);
            list.Add(2);
            list.Add(3);
            list.Remove(3);
            IEnumerator<int> enumerator = list.GetEnumerator();
            enumerator.MoveNext();
            Assert.Equal(2, enumerator.Current);
        }

        [Fact]
        public void RemoveIsCalledShouldDeleteNode()
        {
            SimpleLinkedList<int> list = new SimpleLinkedList<int>();
            list.Add(1);
            list.Add(2);
            list.Add(3);
            list.Remove(list.Last);
            IEnumerator<int> enumerator = list.GetEnumerator();
            enumerator.MoveNext();
            enumerator.MoveNext();
            Assert.Equal(2, enumerator.Current);
            Assert.False(enumerator.MoveNext());
        }

        [Fact]
        public void ContainsIsCalledShouldReturnTrue()
        {
            SimpleLinkedList<int> list = new SimpleLinkedList<int>();
            list.Add(1);
            list.Add(2);
            list.Add(3);
            Assert.True(list.Contains(2));
        }

        [Fact]
        public void ContainsIsCalledShouldReturnFalse()
        {
            SimpleLinkedList<int> list = new SimpleLinkedList<int>();
            list.Add(1);
            list.Add(2);
            list.Add(3);
            Assert.False(list.Contains(67));
        }

        [Fact]
        public void CopyToIsCalledShouldCopyElementsToArray()
        {
            SimpleLinkedList<int> list = new SimpleLinkedList<int>();
            list.Add(1);
            list.Add(2);
            list.Add(3);
            int[] array = new int[3];
            int[] test = new[] { 3, 2, 1 };
            list.CopyTo(array, 0);
            Assert.Equal(array, test);
        }
    }
}
