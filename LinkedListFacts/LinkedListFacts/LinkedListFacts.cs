using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LinkedListFacts
{
    public class LinkedListFacts
    {
        [Fact]
        public void AddIsCalledShouldUpdateList()
        {
            const int test = 1;
            LinkedList<int> list = new LinkedList<int>();
            list.Add(test);
            Assert.Equal(1, list.Last.Data);
            Assert.Equal(0, list.Last.Previous.Data);
            Assert.Equal(0, list.Last.Next.Data);
        }

        [Fact]
        public void AddIsCalledMultipleTimesShouldReturnNodesOrder()
        {
            const int test = 1;
            const int test2 = 2;
            const int test3 = 3;
            LinkedList<int> list = new LinkedList<int>();
            list.Add(test);
            list.Add(test2);
            list.Add(test3);
            IEnumerator enumerator = list.GetEnumerator();
            enumerator.MoveNext();
            Assert.Equal(1, enumerator.Current);
            enumerator.MoveNext();
            Assert.Equal(2, enumerator.Current);
            enumerator.MoveNext();
            Assert.Equal(3, enumerator.Current);
        }

        [Fact]
        public void ContainsIsCalledShouldReturnFalse()
        {
            const int test = 1;
            const int test2 = 2;
            const int test3 = 3;
            LinkedList<int> list = new LinkedList<int>();
            list.Add(test);
            list.Add(test2);
            list.Add(test3);
            Assert.False(list.Contains(4));
        }

        [Fact]
        public void ContainsIsCalledShouldReturnTrue()
        {
            const int test = 1;
            const int test2 = 2;
            const int test3 = 3;
            LinkedList<int> list = new LinkedList<int>();
            list.Add(test);
            list.Add(test2);
            list.Add(test3);
            Assert.True(list.Contains(2));
        }

        [Fact]
        public void CopyToIsCalledShouldCopyDataToArray()
        {
            const int test = 1;
            const int test2 = 2;
            const int test3 = 3;
            LinkedList<int> list = new LinkedList<int>();
            list.Add(test);
            list.Add(test2);
            list.Add(test3);
            int[] array = new int[3];
            int[] testArray = new[] { 1, 2, 3 };
            list.CopyTo(array, 0);
            Assert.Equal(testArray, array);
        }

        [Fact]
        public void ClearIsCalledShouldReturnListToBasicForm()
        {
            const int test = 1;
            const int test2 = 2;
            const int test3 = 3;
            LinkedList<int> list = new LinkedList<int>();
            list.Add(test);
            list.Add(test2);
            list.Add(test3);
            list.Clear();
            Assert.Equal(0, list.Count);
            Assert.Equal(null, list.Last.Next);
            Assert.Equal(null, list.Last.Previous);
        }

        [Fact]
        public void RemoveIsCalledShouldDeleteFirstNode()
        {
            const int test = 1;
            const int test2 = 2;
            const int test3 = 3;
            LinkedList<int> list = new LinkedList<int>();
            list.Add(test);
            list.Add(test2);
            list.Add(test3);
            list.Remove(1);
            IEnumerator enumerator = list.GetEnumerator();
            enumerator.MoveNext();
            Assert.Equal(2, enumerator.Current);
            Assert.Equal(2, list.Last.Previous.Data);
        }

        [Fact]
        public void RemoveIsCalledShouldDeleteSecondNode()
        {
            const int test = 1;
            const int test2 = 2;
            const int test3 = 3;
            LinkedList<int> list = new LinkedList<int>();
            list.Add(test);
            list.Add(test2);
            list.Add(test3);
            list.Remove(2);
            IEnumerator enumerator = list.GetEnumerator();
            enumerator.MoveNext();
            Assert.Equal(1, enumerator.Current);
            enumerator.MoveNext();
            Assert.Equal(3, enumerator.Current);
        }

        [Fact]
        public void RemoveIsCalledShouldDeleteLastNode()
        {
            const int test = 1;
            const int test2 = 2;
            const int test3 = 3;
            LinkedList<int> list = new LinkedList<int>();
            list.Add(test);
            list.Add(test2);
            list.Add(test3);
            list.Remove(3);
            Assert.Equal(1, list.Last.Previous.Data);
            Assert.Equal(2, list.Last.Data);
        }

        [Fact]
        public void RemoveIsCalledShouldReturnTrue()
        {
            const int test = 1;
            const int test2 = 2;
            const int test3 = 3;
            LinkedList<int> list = new LinkedList<int>();
            list.Add(test);
            list.Add(test2);
            list.Add(test3);
            Assert.True(list.Remove(3));
        }

        [Fact]
        public void RemoveIsCalledShouldReturnFalse()
        {
            const int test = 1;
            const int test2 = 2;
            const int test3 = 3;
            LinkedList<int> list = new LinkedList<int>();
            list.Add(test);
            list.Add(test2);
            list.Add(test3);
            Assert.False(list.Remove(4));
        }

        [Fact]
        public void EnumeratorsMoveNextIsCalledShouldReturnTrue()
        {
            const int test = 1;
            const int test2 = 2;
            const int test3 = 3;
            LinkedList<int> list = new LinkedList<int>();
            list.Add(test);
            list.Add(test2);
            list.Add(test3);
            IEnumerator<int> enumerator = list.GetEnumerator();
            Assert.True(enumerator.MoveNext());
        }

        [Fact]
        public void EnumeratorsMoveNextIsCalledShouldReturnFalse()
        {
            const int test = 1;
            const int test2 = 2;
            const int test3 = 3;
            LinkedList<int> list = new LinkedList<int>();
            list.Add(test);
            list.Add(test2);
            list.Add(test3);
            IEnumerator<int> enumerator = list.GetEnumerator();
            enumerator.MoveNext();
            enumerator.MoveNext();
            enumerator.MoveNext();
            Assert.False(enumerator.MoveNext());
        }

        [Fact]
        public void EnumeratorsCurrentIsCalledShouldReturnLastNodeData()
        {
            const int test = 1;
            const int test2 = 2;
            const int test3 = 3;
            LinkedList<int> list = new LinkedList<int>();
            list.Add(test);
            list.Add(test2);
            list.Add(test3);
            IEnumerator<int> enumerator = list.GetEnumerator();
            enumerator.MoveNext();
            enumerator.MoveNext();
            enumerator.MoveNext();
            Assert.Equal(3, enumerator.Current);
        }
    }
}