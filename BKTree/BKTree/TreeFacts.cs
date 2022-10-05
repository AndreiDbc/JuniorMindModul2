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
        public void AddIsCalledShouldAddChildrenToRoot()
        {
            var tree = new Tree(10);
            const string n1 = "some";
            const string n2 = "salmon";
            const string n3 = "soft";
            const string n4 = "same";
            tree.Add(n1);
            tree.Add(n2);
            tree.Add(n3);
            tree.Add(n4);
            IEnumerable<string> words = tree.ReturnWords("sort");
            IEnumerator<string> enumerator = words.GetEnumerator();
            enumerator.MoveNext();
            Assert.Equal("some", enumerator.Current);
            enumerator.MoveNext();
            Assert.Equal("salmon", enumerator.Current);
            enumerator.MoveNext();
            Assert.Equal("soft", enumerator.Current);
            enumerator.MoveNext();
            Assert.Equal("same", enumerator.Current);

        }

        [Fact]
        public void ReturnWordsIsCalledShouldReturnListOfWords()
        {
            var tree = new Tree(2);
            const string n1 = "some";
            const string n2 = "salmon";
            const string n3 = "soft";
            const string n4 = "same";
            const string n5 = "mole";
            const string n6 = "soda";
            tree.Add(n1);
            tree.Add(n2);
            tree.Add(n3);
            tree.Add(n4);
            tree.Add(n5);
            tree.Add(n6);
            IEnumerable<string> words = tree.ReturnWords("sort");
            IEnumerator<string> enumerator = words.GetEnumerator();
            enumerator.MoveNext();
            Assert.Equal("some", enumerator.Current);
            enumerator.MoveNext();
            Assert.Equal("soft", enumerator.Current);
            enumerator.MoveNext();
            Assert.Equal("soda", enumerator.Current);

        }
    }
}
