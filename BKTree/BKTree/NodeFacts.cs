using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BKTree
{
    public class NodeFacts
    {
        [Fact]
        public void GetDistanceIsCalledShouldReturnDistanceBetweenNodes()
        {
            Node first = new Node("Drill");
            Node second = new Node("Dill");
            Assert.Equal(1, first.GetDistance(second));
        }

        [Fact]
        public void ChildAtDistanceIsCalledShouldReturnNode()
        {
            var n1 = new Node("some");
            var n2 = new Node("salmon");
            var n3 = new Node("soft");
            var n4 = new Node("same");
            n1.AddChild(n2);
            n1.AddChild(n3);
            n1.AddChild(n4);
            Assert.Equal(n4, n1.ChildAtDistance(1));
        }
    }
}