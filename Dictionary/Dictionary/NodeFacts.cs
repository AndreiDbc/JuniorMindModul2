using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Dictionary
{
    public class NodeFacts
    {
        [Fact]
        public void ANodeIsCreatedShouldReturnNodeWithValues()
        {
            Node<int> node = new Node<int>(1);
            Assert.Equal(1, node.Data);
            Assert.Equal(null, node.Next);
        }
    }
}
