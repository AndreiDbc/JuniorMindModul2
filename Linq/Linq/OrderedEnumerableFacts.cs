using System;
using System.Collections;
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
            var enumerator = new OrderedEnumerable<string>(
              new List<string> { "bbc", "dd", "a", "ss" },
              Comparer<string>.Default).GetEnumerator();
            enumerator.MoveNext();
            Assert.Equal("a", enumerator.Current);
            enumerator.MoveNext();
            Assert.Equal("bbc", enumerator.Current);
            enumerator.MoveNext();
            Assert.Equal("dd", enumerator.Current);
            enumerator.MoveNext();
            Assert.Equal("ss", enumerator.Current);
        }

        [Fact]
        public void CreateOrderedEnumerableIsCalledShouldReturnNewOrderedEnumerable()
        {
            Assert.Equal(
                new OrderedEnumerable<string>(new List<string> { "bbc", "dd", "a", "ss" }, Comparer<string>.Default).GetType(),
                new OrderedEnumerable<string>(
                new List<string> { "bbc", "dd", "a", "ss" },
                new CustomComparer<string>(Comparer<string>.Default, new ComparerConverter<string, bool>(x => x.EndsWith("b"), Comparer<bool>.Default))).GetType());
        }

        [Fact]
        public void CreateOrderedEnumerableIsCalledKeySelectorIsNullShouldThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => new OrderedEnumerable<string>(new List<string>() { "as", "s" }, Comparer<string>.Default)
            .CreateOrderedEnumerable(null, Comparer<string>.Default, false));
        }
    }
}
