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
        public void ChildAtDistanceIsCalledShouldReturnChild()
        {
            var first = new Node("Drill");
            var second = new Node("Bill");
            var third = new Node("Dill");
            first.AddChild(second);
            first.AddChild(third);
            Assert.Equal(third, first.ChildAtDistance(1));
        }

        [Fact]
        public void ChildAtDistanceIsCalledShouldReturnNull()
        {
            var first = new Node("Drill");
            var second = new Node("Bill");
            var third = new Node("Dill");
            first.AddChild(second);
            first.AddChild(third);
            Assert.Equal(null, first.ChildAtDistance(5));
        }

        [Fact]
        public void AddChildIsCalledShouldAddChild()
        {
            var first = new Node("Drill");
            var second = new Node("Bill");
            var third = new Node("Dill");
            first.AddChild(second);
            first.AddChild(third);
            Assert.Equal(new[] { second, third }, first.Children);
        }

        [Fact]
        public void AddChildIsCalledShouldReturnTrue()
        {
            var first = new Node("Drill");
            var second = new Node("Bill");
            var third = new Node("Dill");
            Assert.True(first.AddChild(second));
            Assert.True(first.AddChild(third));
        }

        [Fact]
        public void AddChildIsCalledChildAllreadyExistsWithDistanceShouldReturnFalse()
        {
            var first = new Node("Drill");
            var second = new Node("Bill");
            var third = new Node("Dill");
            var fourth = new Node("ill");
            first.AddChild(second);
            first.AddChild(third);
            first.AddChild(fourth);
            Assert.Equal(new[] { second, third }, first.Children);
            Assert.False(first.AddChild(fourth));
        }

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