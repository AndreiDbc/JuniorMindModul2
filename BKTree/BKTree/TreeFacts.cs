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
            List<string> words = tree.ReturnWords("sort");
            List<string> toCompare = new List<string>();
            toCompare.Add(n1);
            toCompare.Add(n2);
            toCompare.Add(n3);
            toCompare.Add(n4);
            Assert.Equal(toCompare, words);
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
            List<string> words = tree.ReturnWords("sort");
            List<string> toCompare = new List<string>();
            toCompare.Add(n1);
            toCompare.Add(n3);
            toCompare.Add(n6);
            Assert.Equal(toCompare, words);
        }
    }
}
