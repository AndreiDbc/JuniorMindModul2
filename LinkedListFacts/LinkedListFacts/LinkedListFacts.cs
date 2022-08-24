using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LinkedListFacts
{
    public class LinkedListFacts
    {
        [Fact]
        public void AddIsCalledShouldUpdateList()
        {
            Node<int> node = new Node<int>(1);
            LinkedList<int> list = new LinkedList<int>();
            list.Add(node);
            Assert.Equal(node, list.Last);
        }

        [Fact]
        public void AddIsCalledMultipleTimesShouldReturnNodesOrder()
        {
            Node<int> node1 = new Node<int>(1);
            Node<int> node2 = new Node<int>(2);
            Node<int> node3 = new Node<int>(3);
            LinkedList<int> list = new LinkedList<int>();
            list.Add(node1);
            list.Add(node2);
            list.Add(node3);
            Assert.Equal(node1, list.Head.Next);
            Assert.Equal(node2, list.Last.Previous);
            Assert.Equal(node3, list.Last);
        }
    }
}