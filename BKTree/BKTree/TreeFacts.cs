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
            var tree = new Tree();
            var n1 = new Node("some");
            var n2 = new Node("salmon");
            var n3 = new Node("soft");
            var n4 = new Node("same");
            tree.Add(n1);
            tree.Add(n2);
            tree.Add(n3);
            tree.Add(n4);
            Node[] children = new[] { n2, n3, n4 };
            Assert.Equal(children, n1.Next);
        }

        [Fact]
        public void AddIsCalledShouldAddChildToLeafNode()
        {
            var tree = new Tree();
            var n1 = new Node("some");
            var n2 = new Node("salmon");
            var n3 = new Node("soft");
            var n4 = new Node("same");
            var n5 = new Node("mole");
            var n6 = new Node("soda");
            tree.Add(n1);
            tree.Add(n2);
            tree.Add(n3);
            tree.Add(n4);
            tree.Add(n5);
            tree.Add(n6);
            Node[] children = new[] { n5, n6 };
            Assert.Equal(children, n3.Next);
        }
    }
}
