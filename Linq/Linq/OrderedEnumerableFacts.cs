using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Linq
{
    public class OrderedEnumerableFacts
    {
        [Fact]
        public void GetEnumeratorIsCalledShouldReturnElements()
        {
            var enumerator = new OrderedEnumerable<string, string>(
              new List<string> { "bbc", "dd", "a", "ss" },
              Comparer<string>.Default,
              x => x).GetEnumerator();
            enumerator.MoveNext();
            Assert.Equal("a", enumerator.Current);
            enumerator.MoveNext();
            Assert.Equal("bbc", enumerator.Current);
            enumerator.MoveNext();
            Assert.Equal("dd", enumerator.Current);
            enumerator.MoveNext();
            Assert.Equal("ss", enumerator.Current);
        }
    }
}
