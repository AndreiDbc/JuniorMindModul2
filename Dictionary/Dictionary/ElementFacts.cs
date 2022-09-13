using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Dictionary
{
    public class ElementFacts
    {
        [Fact]
        public void ANodeIsCreatedShouldReturnNodeWithValues()
        {
            Element<int, string> node = new Element<int, string>(1, "a");
            Assert.Equal("a", node.Value);
            Assert.Equal(-1, node.Next);
        }
    }
}
