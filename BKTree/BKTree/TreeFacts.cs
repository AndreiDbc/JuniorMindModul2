using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BKTree
{
    public class TreeFacts
    {
        [Fact]
        public void AddIsCalledShouldAddChildToRoot()
        {
            var tree = new Tree(1);
            tree.Add("abc");
            tree.Add("def");
            var enumerator = tree.GetEnumerator();
            enumerator.MoveNext();
            Assert.Equal("abc", enumerator.Current);
            enumerator.MoveNext();
            Assert.Equal("def", enumerator.Current);

        }

        [Fact]
        public void AddIsCalledShouldAddChildrenToRoot()
        {
            var tree = new Tree(10) { "some", "salmon", "soft", "same" };
            Assert.Equal(new[] { "some", "salmon", "soft", "same" }, tree.Find("sort"));
        }

        [Fact]
        public void FindIsCalledShouldReturnListOfWords()
        {
            var tree = new Tree(2) { "some", "salmon", "soft", "same", "mole", "soda" };
            Assert.Equal(new[] { "some", "soft", "soda" }, tree.Find("sort"));
        }
    }
}
