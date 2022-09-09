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
            Assert.Equal(1, list.Count);
            Assert.Equal(1, list.Last.Data);
            Assert.Equal(null, list.Last.Next);
        }
    }
}
