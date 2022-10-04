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
            const string n1 = "some";
            const string n2 = "salmon";
            const string n3 = "soft";
            const string n4 = "same";
            tree.Add(n1);
            tree.Add(n2);
            tree.Add(n3);
            tree.Add(n4);
            Node[] children = new[] { new Node(n2), new Node(n3), new Node(n4) };
        }

        [Fact]
        public void AddIsCalledShouldAddChildToLeafNode()
        {
            var tree = new Tree();
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
            Node[] children = new[] { new Node(n5), new Node(n6) };
        }
    }
}
